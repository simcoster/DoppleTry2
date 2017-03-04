﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Linq;

namespace Dopple.InstructionNodes
{
    public class LdArgInstructionNode : FunctionArgInstNode
    {
        public LdArgInstructionNode(Instruction instruction, MethodDefinition method) : base(instruction, method)
        {
        }
        protected override bool TryGetCodeArgIndex(Instruction instruction, out int index)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Ldarg_0:
                    index = 0;
                    return true;
                case Code.Ldarg_1:
                    index = 1;
                    return true;
                case Code.Ldarg_2:
                    index = 2;
                    return true;
                case Code.Ldarg_3:
                    index = 3;
                    return true;
                default:
                    index = -1;
                    return false;
            }
        }
        public override int StackPopCount
        {
            get
            {
                if (InliningProperties.Inlined)
                {
                    return 0;
                }
                return base.StackPopCount;
            }

            protected set
            {
                base.StackPopCount = value;
            }
        }

        public override int DataFlowDataProdivderIndex
        {
            get
            {
                return 0;
            }
        }
    }

    public class StArgInstructionNode : FunctionArgInstNode
    {
        public StArgInstructionNode(Instruction instruction, MethodDefinition method) : base(instruction, method) { }

        public override int DataFlowDataProdivderIndex
        {
            get
            {
                return 0;
            }
        }
    }

    public class StThisArgInstructionWrapper : StArgInstructionNode
    {
        public StThisArgInstructionWrapper(Instruction instruction, MethodDefinition method) : base(instruction, method)
        {
            ArgIndex = 0;
            ArgName = "this";
            ArgType = method.DeclaringType;
        }
    }

    public abstract class FunctionArgInstNode : InstructionNode, IDataTransferingNode
    {
        public FunctionArgInstNode(Instruction instruction, MethodDefinition method) : base(instruction, method)
        {
            ArgIndex = GetArgIndex(instruction, method);
            string argNameTemp;
            TypeReference argTypeTemp;
            ParamDefinition = GetArgNameAndType(ArgIndex, method, out argNameTemp, out argTypeTemp);
            ArgName = argNameTemp;
            ArgType = argTypeTemp;
        }

        private static ParameterDefinition GetArgNameAndType(int argIndex, MethodDefinition method, out string argName, out TypeReference argType)
        {
            int parameterArrayIndex;
            if (method.IsStatic)
            {
                parameterArrayIndex = argIndex;
            }
            else
            {
                parameterArrayIndex = argIndex - 1;
                if (parameterArrayIndex == -1)
                {
                    argName = "this";
                    argType = method.DeclaringType;
                    return null;
                }
            }
            argName = method.Parameters[parameterArrayIndex].Name;
            argType = method.Parameters[parameterArrayIndex].ParameterType;
            return method.Parameters[parameterArrayIndex];
        }

        public int ArgIndex { get; set; }
        public TypeReference ArgType { get; set; }
        public string ArgName { get; set; }
        public ParameterDefinition ParamDefinition { get; private set; }
        public abstract int DataFlowDataProdivderIndex { get; }

        private int GetArgIndex(Instruction instruction, MethodDefinition method)
        {
            int indexByCode = 0;
            if (TryGetCodeArgIndex(instruction, out indexByCode) != false)
            {
                return indexByCode;
            }
            int indexByOperand;
            if (instruction.Operand is ValueType)
                indexByOperand = Convert.ToInt32(instruction.Operand);
            else if (instruction.Operand is VariableDefinition)
                indexByOperand =((VariableDefinition)instruction.Operand).Index;
            else if (instruction.Operand is ParameterDefinition)
                indexByOperand = ((ParameterDefinition)instruction.Operand).Index;
            else
            {
                throw new Exception("shouldn't get here");
            }
            if (method.IsStatic)
            {
                return indexByOperand;
            }
            else
            {
                return indexByOperand + 1;
            }
        }

        protected virtual bool TryGetCodeArgIndex(Instruction instruction, out int index)
        {
            index = -1;
            return false;
        }
    }
}