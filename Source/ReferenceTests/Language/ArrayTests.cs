using System;
using NUnit.Framework;
using System.Management.Automation;
using TestPSSnapIn;
using System.Runtime.InteropServices;

namespace ReferenceTests
{
    public class ArrayTests : ReferenceTestBase
    {
        [TestCase("1,2,3")]
        [TestCase(",2")]
        [TestCase("@('foo',2,3)")]
        public void ArrayExpressionIsArrayType(string definition)
        {
            var cmd = String.Format("$a = {0}; $a.GetType().FullName", definition);
            var result = ReferenceHost.Execute(cmd);
            Assert.AreEqual(NewlineJoin(typeof(object[]).FullName), result);
        }

        [Test]
        public void EmptyArrayWorks()
        {
            var cmd = "$a = @(); $a.Length";
            Assert.AreEqual(NewlineJoin("0"), ReferenceHost.Execute(cmd));
        }

        [Test]
        public void ArrayInVariableGetsEvaluatedWhenPassedToPipeline()
        {
            var cmd = String.Format("$a = @(1,2,3); $a");
            var results = ReferenceHost.RawExecute(cmd);
            Assert.AreEqual(3, results.Count);
        }

        [Test]
        public void NestedOneDimensionalArrayEvaluates()
        {
            var results = ReferenceHost.RawExecute("@(@(@('foo')))");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(PSObject.AsPSObject("foo"), results[0]);
        }

        [Test]
        public void NestedOneDimensionalArrayEvaluatesInArrayList()
        {
            var cmd = "$a = 1,2,@(4),,3; $a.Length; $a[2].GetType().FullName; $a[3].GetType().FullName";
            var objArrayName = typeof(object[]).FullName;
            Assert.AreEqual(NewlineJoin("4",objArrayName, objArrayName), ReferenceHost.Execute(cmd));
        }

        [Test]
        public void ElementsFromPipelineAreStoredAsArray()
        {
            var result = ReferenceHost.Execute(NewlineJoin(
                "$a = @('foo', 'bar', 'baz') | " + CmdletName(typeof(TestCmdletPhasesCommand)),
                "$a.GetType().FullName"
            ));
            Assert.AreEqual(NewlineJoin(typeof(object[]).FullName), result);
        }

        [Test]
        public void ArrayInParenthesisIsStillArray()
        {
            var result = ReferenceHost.Execute("(@(1,2)).GetType().FullName");
            Assert.AreEqual(NewlineJoin(typeof(object[]).FullName), result);
        }

        [Test] // issue #116
        public void SingleArrayElementIsStillArray()
        {
            var result = ReferenceHost.Execute("@(1).GetType().FullName; @(1).Count");
            Assert.AreEqual(NewlineJoin(typeof(object[]).FullName, "1"), result);
        }

        [Test]
        public void NestedTwoDimensionalArrayWorks()
        {
            var results = ReferenceHost.RawExecute("@(@(@('foo')), @(@('bar')))");
            Assert.AreEqual(2, results.Count);
            var expected = new[] { "foo", "bar" };
            for (int i = 0; i < 2; i++)
            {
                var psobjSubarray = results[i];
                Assert.IsInstanceOf<PSObject>(psobjSubarray);
                var subarray = ((PSObject)psobjSubarray).BaseObject as object[];
                Assert.NotNull(subarray);
                Assert.AreEqual(PSObject.AsPSObject(expected[i]), subarray[0]);

            }
        }
    }
}

