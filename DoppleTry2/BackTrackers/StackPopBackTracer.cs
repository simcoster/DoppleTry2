﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil.Cil;
using DoppleTry2.InstructionNodes;

namespace DoppleTry2.BackTrackers
{
    public class StackPopBackTracer : BackTracer
    {
        /// <summary>
        /// This function uses both recursion And mutual recursion with AddBackDataflowConnections
        /// If the search comes accross stackpop instruction wrappers that have not been resolved yet, they must be so
        /// only then can it resolve its
        /// The reason is the stack structure - push will be pulled by its nearest neighbour
        /// Since its difficult to know ahead who will those couples be, we just start going and then deal with
        /// those when they come by.
        /// </summary>
        /// <param name="instructionWrappers"></param>
        /// <param name="predicate"></param>
        /// <param name="startInstruction"></param>
        /// <param name="visitedNodes"></param>
        /// <returns></returns>
        public IEnumerable<InstructionNode> SearchAndAddDataflowInstrcutions(InstructionNode startInstruction, List<InstructionNode> visitedNodes = null)
        {
            var foundInstructions = new List<InstructionNode>();
            if (visitedNodes == null)
            {
                visitedNodes = new List<InstructionNode>();
            }
            foreach (var backNode in startInstruction.ProgramFlowBackRoutes)
            {
                if (visitedNodes.Contains(backNode))
                {
                    continue;
                }
                else
                {
                    visitedNodes.Add(backNode);
                }
                bool mustRecurseFirst = backNode.StackPopCount != 0;
                if (mustRecurseFirst)
                {
                    AddBackDataflowConnections(backNode);
                }
                if (backNode.StackPushCount >0 )
                {
                    foundInstructions.Add(backNode);
                }
                else
                {
                    foundInstructions.AddRange(SearchAndAddDataflowInstrcutions(backNode, visitedNodes));
                }
            }
            return foundInstructions;
        }

        public override void AddBackDataflowConnections(InstructionNode currentInst)
        {
            for (int i = 0; i < currentInst.StackPopCount; i++)
            {
                var argumentGroup = SearchAndAddDataflowInstrcutions(currentInst);
                if (argumentGroup.Count() ==0)
                {
                    throw new Exception("Couldn't find back data connections");
                }
                if (CodeGroups.CallCodes.Contains(currentInst.Instruction.OpCode.Code))
                {
                    currentInst.DataFlowBackRelated.AddTwoWay(argumentGroup, currentInst.StackPopCount-i-1);
                }
                else
                {
                    currentInst.DataFlowBackRelated.AddTwoWaySingleIndex(argumentGroup);
                }
                foreach (InstructionNode arg in argumentGroup)
                {
                    arg.StackPushCount--;
                }
            }
            currentInst.StackPopCount = 0;
        }

        private void BackupCallsProgramFlow(Dictionary<InstructionNode, List<InstructionNode>> CallWrappersFlowBackup)
        {
            foreach (var callInstWrapper in InstructionNodes.Where(x => CodeGroups.CallCodes.Contains(x.Instruction.OpCode.Code)))
            {
                CallWrappersFlowBackup.Add(callInstWrapper, new List<InstructionNode>(callInstWrapper.ProgramFlowBackRoutes));
                callInstWrapper.ProgramFlowBackRoutes.Clear();
            }
        }

        private void RestoreCallsProgramFlow(Dictionary<InstructionNode, List<InstructionNode>> CallWrappersFlowBackup)
        {
            foreach (var callInstWrapper in InstructionNodes.Where(x => CodeGroups.CallCodes.Contains(x.Instruction.OpCode.Code)))
            {
                callInstWrapper.ProgramFlowBackRoutes.AddTwoWay(CallWrappersFlowBackup[callInstWrapper]);
            }
        }


        public override Code[] HandlesCodes => typeof(OpCodes).GetFields()
                    .Select(x => x.GetValue(null))
                    .Cast<OpCode>()
                    .Where(x => x.StackBehaviourPop != StackBehaviour.Pop0)
                    .Select(x => x.Code)
                    .ToArray();

        public StackPopBackTracer(List<InstructionNode> instructionNodes) : base(instructionNodes)
        {
        }
    }
}