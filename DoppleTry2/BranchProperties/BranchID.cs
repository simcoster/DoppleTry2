﻿using Dopple.InstructionNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dopple.Tracers.ForwardTracers;

namespace Dopple.BranchPropertiesNS
{
    public class BranchID
    {
        private static object _LockGlobalIndex = new object();
        private static int GlobalIndex = 0;
        public BranchID(InstructionNode originatingNode)
        {
            lock(_LockGlobalIndex)
            {
                Index = GlobalIndex;
                GlobalIndex++;
            }
            OriginatingNode = originatingNode;
        }
        public int Index { get; private set; }
        public virtual BranchType BranchType { get; set; }
        public InstructionNode OriginatingNode { get; set; }
        public PairedBranchIndex PairedBranchesIndex { get; set; } = PairedBranchIndex.First;
        public List<InstructionNode> BranchNodes { get; } = new List<InstructionNode>();

        public virtual void RemoveNode(InstructionNode instructionNode)
        {
            OriginatingNode.BranchProperties.AffectedNodes.Remove(instructionNode);
        }
    }

    public class BaseBranch : BranchID
    {
        public BaseBranch() : base(null)
        {
        }

        public override void RemoveNode(InstructionNode instructionNode)
        {
        }
        public override BranchType BranchType
        {
            get
            {
                return BranchType.Exit;
            }
            set
            {
            }
        }
    }
}
