﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using Dopple.InstructionNodes;

namespace Dopple.BackTracers
{
    class TypedReferenceBackTracer : SingeIndexBackTracer
    {
        public TypedReferenceBackTracer(List<InstructionNode> instructionsWrappers) : base(instructionsWrappers)
        {
        }

        protected override IEnumerable<InstructionNode> GetDataflowBackRelatedArgGroup(InstructionNode instWrapper)
        {
            return _SingleIndexBackSearcher.SearchBackwardsForDataflowInstrcutions(x => x.Instruction.OpCode.Code == Code.Mkrefany, instWrapper);
        }


        public override Code[] HandlesCodes => new[] {Code.Refanyval};
    }
}
