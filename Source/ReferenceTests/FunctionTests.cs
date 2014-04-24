// Copyright (C) Pash Contributors. License: GPL/BSD. See https://github.com/Pash-Project/Pash/
using System;
using NUnit.Framework;

namespace ReferenceTests
{
    [TestFixture]
    public class FunctionTests : ReferenceTestBase
    {
        [TearDown]
        public void Cleanup()
        {
            RemoveCreatedScripts();
        }

        [Test]
        public void FunctionDeclarationWithoutParameterList()
        {
            Assert.DoesNotThrow(
                delegate() { ReferenceHost.Execute("function f() { 'x' }"); }
            );
        }
    }
}

