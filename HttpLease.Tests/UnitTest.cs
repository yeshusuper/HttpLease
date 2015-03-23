using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpLease.Tests
{
    [TestClass]
    public class UnitTest
    {
        private IHttp _Http;

        [TestInitialize]
        public void Initialize()
        {
            GlobalConfig.Config.Host = "http://127.0.0.1:5698";
            _Http = HttpLease.Get<IHttp>();
        }

        [TestMethod]
        public void Get_Api_Test()
        {
            var result = _Http.Test(1, "234");
            Assert.AreEqual("{\"id\":1,\"value\":\"234\"}", result);
        }

        [TestMethod]
        public void Get_Api_Test_With_Default_Path()
        {
            var result = _Http.Test2(2, "2345");
            Assert.AreEqual("{\"id\":2,\"value\":\"2345\"}", result);
        }

        [TestMethod]
        public void Get_Api_Test_With_Path()
        {
            var result = _Http.Test3(3, "23456");
            Assert.AreEqual("{\"id\":3,\"value\":\"23456\"}", result);
        }
    }
}
