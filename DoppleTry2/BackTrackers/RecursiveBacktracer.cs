﻿using System.Collections.Generic;
using Dopple.InstructionNodes;

namespace Dopple.BackTracers
{
    public abstract class RecursiveBacktracer : BackTracer
    {
        protected BackTraceManager backtraceManager;

        public RecursiveBacktracer(List<InstructionNode> instructionNodes, BackTraceManager backtraceManager) : base(instructionNodes)
        {
            this.backtraceManager = backtraceManager;
        }
    }
}