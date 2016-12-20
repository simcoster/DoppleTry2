using System;
using DoppleTry2.InstructionWrappers;
using GraphSimilarity.EditOperations;
using System.Collections.Generic;
using System.Linq;

namespace GraphSimilarity
{
    internal abstract class NodeEditOperation : EditOperation
    {
        public NodeEditOperation(List<InstructionWrapper> graph, InstructionWrapper node) : base(graph)
        {
            Node = node;
        }

        public InstructionWrapper Node { get; private set; }
        public CalculatedOperation ComputeAndCommit()
        {
            var computedNodeOperation = new CalculatedOperation();
            computedNodeOperation.NodeOperation = this;
            computedNodeOperation.EdgeOperations = GetEdgeOperations();
            computedNodeOperation.Cost = computedNodeOperation.NodeOperation.Cost + computedNodeOperation.EdgeOperations.Sum(x => x.Cost);
            return computedNodeOperation;
        }
        protected abstract List<EdgeEditOperation> GetEdgeOperations();

    }
}