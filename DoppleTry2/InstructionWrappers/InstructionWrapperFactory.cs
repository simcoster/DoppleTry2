﻿using DoppleTry2.InstructionWrappers;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

namespace DoppleTry2.InstructionWrappers
{
    public class InstructionWrapperFactory
    {
        public static InstructionWrapper GetInstructionWrapper(Instruction instruction, MethodDefinition method)
        {
            if (CodeGroups.CallCodes.Contains(instruction.OpCode.Code)
                && instruction.Operand is MethodDefinition)
            {
                return new CallInstructionWrapper(instruction, method);
            }
            else if (CodeGroups.LdArgCodes.Contains(instruction.OpCode.Code))
            {
                return new LdArgInstructionWrapper(instruction, method);
            }
            else if (CodeGroups.StArgCodes.Contains(instruction.OpCode.Code))
            {
                return new StArgInstructionWrapper(instruction, method);
            }
            else if (CodeGroups.LdLocCodes.Contains(instruction.OpCode.Code))
            {
                return new LocationLoadInstructionWrapper(instruction, method);
            }
            else if (CodeGroups.StLocCodes.Contains(instruction.OpCode.Code))
            {
                return new LocationStoreInstructionWrapper(instruction, method);
            }
            else if (CodeGroups.LdImmediateFromOperandCodes.Concat(CodeGroups.LdImmediateValueCodes).Contains(instruction.OpCode.Code))
            {
                return new LdImmediateInstWrapper(instruction, method);
            }
            else if (CodeGroups.LdElemCodes.Contains(instruction.OpCode.Code))
            {
                return new LdElemInstructionWrapper(instruction, method);
            }
            else
            {
                return new InstructionWrapper(instruction, method);
            }
        }
    }
}