﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dopple.InstructionModifiers;
using Mono.Cecil.Cil;
using Dopple.InstructionNodes;

namespace Dopple.BackTracers
{
    public abstract class BackTracer
    {
        protected InstructionNode Instruction;
        protected abstract void InnerAddBackDataflowConnections(InstructionNode currentInst);
        public void AddBackDataflowConnections(InstructionNode currentInst)
        {
            InnerAddBackDataflowConnections(currentInst);
        }


        protected virtual bool HasBackDataflowNodes { get; } = true;

        public abstract Code[] HandlesCodes { get; }
    }
}
