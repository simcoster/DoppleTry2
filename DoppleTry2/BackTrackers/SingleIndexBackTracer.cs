﻿using Dopple.InstructionNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dopple.BackTracers
{
    public abstract class SingeIndexBackTracer : BackTracer
    {
        protected IEnumerable<IEnumerable<InstructionNode>> GetDataflowBackRelated(InstructionNode instNode)
        {
            return new List<List<InstructionNode>>() { { GetDataflowBackRelatedArgGroup(instNode).ToList() } };
        }
        protected override void InnerAddBackDataflowConnections(InstructionNode currentInst)
        {        
            IEnumerable<IEnumerable<InstructionNode>> backRelatedGroups = GetDataflowBackRelated(currentInst);

            foreach (var backRelatedGroup in backRelatedGroups)
            {
                currentInst.DataFlowBackRelated.AddTwoWaySingleIndex(backRelatedGroup);
            }
        }

        protected abstract IEnumerable<InstructionNode> GetDataflowBackRelatedArgGroup(InstructionNode instructionNode);

       
    }
}
