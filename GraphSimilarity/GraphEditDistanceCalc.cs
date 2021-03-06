﻿using C5;
using Dopple;
using Dopple.InstructionNodes;
using GraphSimilarity.EditOperations;
using Mono.Cecil.Cil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSimilarity
{
    public static class GraphEditDistanceCalc
    {
        public const int NodeAdditionCost = 1;
        public const int NodeDeletionCost = 1;
        public const int NodeLabelSubstitutionCost = 1;

        public static EditPath GetEditDistance(List<InstructionNode> sourceGraph, List<InstructionNode> targetGraph)
        {
            http://www.springer.com/cda/content/document/cda_downloaddocument/9783319272511-c2.pdf?SGWID=0-0-45-1545097-p177820399
                 //step 1 = take all the first graph nodes
                 //step 2 = concider for each one, replacing with each one of the second graph
                 //step 3 = concider for each one, deleting
                 //continue until no more nodes left of the first graph

            // the code that you want to measure comes here

            int index = 0;
            var pathsToConcider = new DuplicateKeySortedDictionary();
            pathsToConcider.Add(new EditPath(sourceGraph, targetGraph));
            var cheapestPaths = pathsToConcider.First().Value;
            EditPath cheapestPath = cheapestPaths[0];
            while (true)
            {
                if (cheapestPath.SourceNodesLeftToResolve.Count != 0)
                {
                    List<CalculatedOperation> possibleOperations = GetPossibleSubsAndDelete(cheapestPath);
                    var tempAddedPaths = new ConcurrentBag<EditPath>();
                    Parallel.ForEach(possibleOperations, (possibleOperation) =>
                    {
                       tempAddedPaths.Add(cheapestPath.CloneWithEditOperation(possibleOperation));
                    });
                    foreach (var pathToConcider in tempAddedPaths)
                    {
                        pathsToConcider.Add(pathToConcider);
                    }
                }
                else
                {
                    foreach (var nodeToAdd in cheapestPath.TargetNodesLeftToResolve)
                    {
                        CalculatedOperation nodeAddition = new NodeAddition(cheapestPath.Graph, nodeToAdd, cheapestPath.EdgeAdditionsPending).GetCalculated();
                        EditPath additionPath = cheapestPath.CloneWithEditOperation(nodeAddition);
                        pathsToConcider.Add(additionPath);
                    }
                }
                var oldCheapestPath = cheapestPath;
                pathsToConcider.Remove(cheapestPath);
                cheapestPath = pathsToConcider.First().Value[0];
                if (cheapestPath.HeuristicCost == 0)
                {
                    return cheapestPath;
                }
            }
        }

        private static List<CalculatedOperation> GetPossibleSubsAndDelete(EditPath currentPath)
        {
            var calculatedOperations = new ConcurrentBag<CalculatedOperation>();
            Parallel.ForEach(currentPath.SourceNodesLeftToResolve, (sourceNode) =>
            {
                var codeGroup = codeGroups.FirstOrDefault(x => x.Contains(sourceNode.Instruction.OpCode.Code));
                if (codeGroup == null)
                {
                    codeGroup = new Code[] { sourceNode.Instruction.OpCode.Code };
                }
                var possibleSubs = currentPath.TargetNodesLeftToResolve.Where(x => codeGroup.Contains(x.Instruction.OpCode.Code));
                Parallel.ForEach(currentPath.TargetNodesLeftToResolve, (targetNode) =>
                {
                    var nodeSubstitution = new NodeSubstitution(currentPath.Graph, currentPath.EdgeAdditionsPending, sourceNode, targetNode);
                    calculatedOperations.Add(nodeSubstitution.GetCalculated());
                });
                var nodeDeletion = new NodeDeletion(currentPath.Graph, sourceNode);
                calculatedOperations.Add(nodeDeletion.GetCalculated());
            });
            return calculatedOperations.ToList();
        }

        static readonly List<Code[]> codeGroups = typeof(CodeGroups).GetFields().Select(x => x.GetValue(null)).Cast<Code[]>().ToList();

    }
}
