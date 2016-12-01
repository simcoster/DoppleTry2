﻿using System.Collections.Generic;
using System.Linq;
using DoppleTry2.InstructionModifiers;
using Mono.Cecil.Cil;
using DoppleTry2.InstructionWrappers;

namespace DoppleTry2.ProgramFlowHanlder
{
    abstract class ProgramFlowHandler
    {
        public static void TwoWayLinkExecutionPath(InstructionWrapper backInstruction, InstructionWrapper forwardInstruction)
        {
            backInstruction.ForwardProgramFlow.Add(forwardInstruction);
            forwardInstruction.BackProgramFlow.Add(backInstruction);
        }

        public List<InstructionWrapper> GetAllPreviousConnected(InstructionWrapper startInstruction, List<InstructionWrapper> visited = null)
        {
            if (visited == null)
            {
                visited = new List<InstructionWrapper>();
            }
            List<InstructionWrapper> prevInstructions = new List<InstructionWrapper>();
            if (startInstruction.BackProgramFlow.Count == 0)
            {
                return prevInstructions;
            }
            if (CodeGroups.CallCodes.Concat(new[] { Code.Ret }).Contains( startInstruction.Instruction.OpCode.Code ))
            {
                return prevInstructions;
            }
            if (visited.Contains(startInstruction))
            {
                return prevInstructions;
            }
            visited.Add(startInstruction);

            var recursivePrevConnected = startInstruction.BackProgramFlow.SelectMany(x => GetAllPreviousConnected(x, visited));
            prevInstructions.AddRange(recursivePrevConnected);
            prevInstructions.Add(startInstruction);
            return prevInstructions;
        }

        public abstract Code[] HandledCodes { get; }

        public void SetForwardExecutionFlowInsts(InstructionWrapper wrapperToModify, List<InstructionWrapper> instructionWrappers)
        {
            if (wrapperToModify.ProgramFlowResolveDone)
            {
                return;
            }
            SetForwardExecutionFlowInstsInternal(wrapperToModify, instructionWrappers);
            wrapperToModify.ProgramFlowResolveDone = true;
        }

        protected abstract void SetForwardExecutionFlowInstsInternal(InstructionWrapper wrapperToModify, List<InstructionWrapper> instructionWrappers);
    }
}
