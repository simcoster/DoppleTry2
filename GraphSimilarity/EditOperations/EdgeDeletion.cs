﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoppleTry2.InstructionWrappers;
using DoppleTry2;

namespace GraphSimilarity.EditOperations
{
    class EdgeDeletion : EdgeEditOperation
    {
        public EdgeDeletion(List<InstructionWrapper> graph, GraphEdge edge) : base(graph, edge)
        {
        }

        public override int Cost
        {
            get
            {
                return 1;
            }
        }

        public override string Name
        {
            get
            {
                return "Edge Deletion";
            }
        }

        public override void Commit()
        {
            BackArgList backNodes = Edge.DestinationNode.BackDataFlowRelated;
            backNodes.RemoveTwoWay(backNodes.First(x => x.Argument == Edge.SourceNode));
        }
    }
}
