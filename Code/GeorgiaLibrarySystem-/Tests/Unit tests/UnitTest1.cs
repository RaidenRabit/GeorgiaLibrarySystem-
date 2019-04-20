using System;
using GtlService;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            int c = 4;
            String b = "You entered: " + c;
            IService1 a = new Service1();

            Assert.IsTrue(a.GetData(c).Equals(b));
        }
    }
}