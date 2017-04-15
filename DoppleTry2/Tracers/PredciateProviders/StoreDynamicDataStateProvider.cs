﻿using Dopple.BranchPropertiesNS;
using Dopple.InstructionNodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dopple.Tracers.PredciateProviders
{
    abstract class StoreDynamicDataStateProvider
    {
        internal StoreDynamicDataStateProvider(InstructionNode storeNode)
        {
            StoreNode = storeNode;
        }
        public abstract bool IsLoadNodeMatching(InstructionNode loadNode);
        public InstructionNode StoreNode { get; private set; }
        public Guid MyGuid { get; set; } = Guid.NewGuid();
        public static StoreDynamicDataStateProvider GetMatchingStateProvider(InstructionNode storeNode)
        {
            if (storeNode is StElemInstructionNode)
            {
                return new StElemStateProvider(storeNode);
            }
            if (storeNode is StoreFieldNode)
            {
                return new StoreFieldStateProvider(storeNode);
            }
            if (storeNode is StoreStaticFieldNode)
            {
                return new StoreStaticFieldStateProvider(storeNode);
            }
            return null;
        }
        internal void OverrideAnother(StoreDynamicDataStateProvider partiallyOverrided, out bool completelyOverrides)
        {
            if (partiallyOverrided.GetType() != this.GetType())
            {
                completelyOverrides = false;
                return;
            }
            if (partiallyOverrided.StoreNode == StoreNode)
            {
                completelyOverrides = true;
                return;
            }
            OverrideAnotherInternal(partiallyOverrided, out completelyOverrides);
        }
        protected abstract void OverrideAnotherInternal(StoreDynamicDataStateProvider overrideCandidate, out bool completelyOverrides);

    }


  
}
