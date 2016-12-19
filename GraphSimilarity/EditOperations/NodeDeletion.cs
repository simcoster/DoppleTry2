﻿using DoppleTry2;
using DoppleTry2.InstructionWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSimilarity.EditOperations
{
    internal class NodeDeletion : NodeEditOperation
    {
        public NodeDeletion(List<InstructionWrapper> graph) : base(graph)
        {
        }

        public override int Cost
        {
            get
            {
                return 2;
            }
        }

        public override string Name
        {
            get
            {
                return "NodeDeletion";
            }
        }

        protected override List<EdgeEditOperation> GetEdgeOperations()
        {
            var relatedEdgeOperations = new List<EdgeEditOperation>();
            InstructionWrapper nodeToRemove = InstructionWrapper;
            foreach (var backNode in nodeToRemove.BackDataFlowRelated.ToArray())
            {
                var tempEdgeDeletion = new EdgeDeletion(graph);
                tempEdgeDeletion.Edge = new GraphEdge(backNode.Argument, nodeToRemove);
                nodeToRemove.BackDataFlowRelated.RemoveTwoWay(backNode);
                relatedEdgeOperations.Add(tempEdgeDeletion);
            }
            foreach (var forwardNode in nodeToRemove.ForwardDataFlowRelated.ToArray())
            {
                var tempEdgeDeletion = new EdgeDeletion(graph);
                tempEdgeDeletion.Edge = new GraphEdge(forwardNode, nodeToRemove);
                IndexedArgument backRelatedToRemove = forwardNode.BackDataFlowRelated.First(x => x.Argument == nodeToRemove);
                forwardNode.BackDataFlowRelated.RemoveTwoWay(backRelatedToRemove);
                relatedEdgeOperations.Add(tempEdgeDeletion);
            }
            return relatedEdgeOperations;
        }
    }
}