﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSimilarityByMatching
{
    public class LabeledVertex
    {
        public Code Opcode { get; set; }
        public object Operand { get; set; }
        public int Index { get; set; }
        public List<LabeledEdge> BackEdges { get; set; } = new List<LabeledEdge>();
        public List<LabeledEdge> ForwardEdges { get; set; } = new List<LabeledEdge>();
        public List<SmallBigLinkEdge> PairingEdges { get; set; } = new List<SmallBigLinkEdge>(); 
        public MethodDefinition Method { get; set; }
    }
    public class MultiNodeLabeledVertex : LabeledVertex
    {
        public List<LabeledVertex> AdditionalVertexes = new List<LabeledVertex>();
    }
}
