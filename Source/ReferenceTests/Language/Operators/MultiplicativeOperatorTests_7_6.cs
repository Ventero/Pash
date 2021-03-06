using System;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace ReferenceTests
{
    [TestFixture]
    public class MultiplicativeOperatorTests_7_6 : ReferenceTestBase
    {
        [TestCase("12 * -10L", (long) -120)]
        [TestCase("10.5 * 13", (double) 136.5)]
        [TestCase("12 * \"0xabc\"", (int) 32976)]
        [TestCase("12 * \"+0xabc\"", (int) 32976)]
        [TestCase("12 * \"-0xabc\"", (int) -32976)]
        public void Multiplication_Spec_7_6_1(string cmd, object result)
        {
            ExecuteAndCompareTypedResult(cmd, result);
        }

        [Test]
        public void Multiplication_Spec_7_6_1_decimal()
        {
            ExecuteAndCompareTypedResult("-10.300D * 12", (decimal) -123.600m);
        }

        [TestCase("2*2", 2.0d)]
        [TestCase("8/2", 2.0d)]
        [TestCase("9%5", 2.0d)]
        public void MultiplicativeArgumentExpressionWorks(string argExp, object expected)
        {
            var cmd = String.Format("[Math]::Sqrt({0})", argExp);
            ExecuteAndCompareTypedResult(cmd, expected);
        }

        [TestCase("\"red\" * \"3\"", "redredred")]
        [TestCase("\"red\" * 4", "redredredred")]
        [TestCase("\"red\" * 0", "")]
        [TestCase("\"red\" * 2.7", "redredred")]
        public void StringReplication_Spec_7_6_2(string cmd, string result)
        {
            ExecuteAndCompareTypedResult(cmd, result);
        }

        public void StringReplication_Spec_7_6_2_decimal()
        {
            ExecuteAndCompareTypedResult("\"red\" * 2.3450D", "redred");
        }

        [TestCase("$a * \"3\"", new object[] {10, 20, 10, 20, 10, 20})]
        [TestCase("$a * 4", new object[] {10, 20, 10, 20, 10, 20, 10, 20})]
        [TestCase("$a * 0", new object[] {})]
        [TestCase("$a * 2.7", new object[] {10, 20, 10, 20, 10, 20})]
        // The following is explicit as PS doesn't match its own specification
        [TestCase("(new-object 'System.Double[,]' 2,3) * 2", new object[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0}, Explicit = true)]
        public void ArrayReplication_Spec_7_6_3(string cmd, object[] result)
        {
            cmd = "$a = new-object System.Int32[] 2; $a[0] = 10; $a[1] = 20;" + cmd;
            ExecuteAndCompareTypedResult(cmd, result);
        }

        [Test]
        public void ArrayReplication_Spec_7_6_3_decimal()
        {
            var cmd = "$a = new-object System.Int32[] 2; $a[0] = 10; $a[1] = 20; $a * 2.3450D";
            ExecuteAndCompareTypedResult(cmd, new object[] {10, 20, 10, 20});
        }

        [Test]
        public void ArrayReplicationKeepsType()
        {
            var cmd = "(([byte[]]5)*2).GetType().FullName";
            var result = ReferenceHost.Execute(cmd);
            Assert.AreEqual(NewlineJoin(typeof(byte[]).FullName), result);
        }

        [TestCase("10/-10", (int) -1)]
        [TestCase("12/-10", (double) -1.2)]
        [TestCase("12/10.6", (double) 1.13207547169811)]
        [TestCase("12/\"0xabc\"", (double) 0.00436681222707424)]
        [TestCase("12/\"+0xabc\"", (double) 0.00436681222707424)]
        [TestCase("12/\"-0xabc\"", (double) -0.00436681222707424)]
        [TestCase("100000000000001 / 100000000000000", (double) 1.00000000000001)]
        public void Division_Spec_7_6_4(string cmd, object result)
        {
            ExecuteAndCompareTypedResult(cmd, result);
        }

        [Test]
        public void Division_Spec_7_6_4_decimal()
        {
            ExecuteAndCompareTypedResult("12/-10.0D", (decimal) -1.2m);
        }

        [TestCase("10 % 3", (int) 1)]
        [TestCase("10.0 % 0.33", (double) 0.1)]
        public void Remainder_Spec_7_6_5(string cmd, object result)
        {
            ExecuteAndCompareTypedResult(cmd, result);
        }

        [Test]
        public void Remainder_Spec_7_6_5_decimal()
        {
            ExecuteAndCompareTypedResult("10.00D % \"0x4\"", (decimal) 2.00m);
        }

        [Test]
        public void Remainder_Spec_7_6_5_decimal2()
        {
            ExecuteAndCompareTypedResult("10.00D % 0.33D", (decimal) 0.10m);
        }
    }
}

