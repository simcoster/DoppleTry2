﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dopple;
using Mono.Cecil;
using System.Linq;
using System.Collections.Generic;
using Dopple.InstructionNodes;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace GraphBuilderTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSimpleAdd()
        {
            HelperFuncs.TestFunction("SimpleAdd");
        }


        [TestMethod]
        public void TestSimpleForLoop()
        {
            HelperFuncs.TestFunction("SimpleForLoop");
        }


        [TestMethod]
        public void TestSimpleWhileLoop()
        {
            HelperFuncs.TestFunction("SimpleWhile");
        }

        [TestMethod]
        public void TestMixedConditions()
        {
            HelperFuncs.TestFunction("ConpoundConditions");
        }

        [TestMethod]
        public void TestRecursionSimple()
        {
            HelperFuncs.TestFunction("SimpleRecursion");
        }

        [TestMethod]
        public void TestOutParam()
        {
            Assert.Fail("need to implement out param with LdLoc.a");
        }
    }
}
