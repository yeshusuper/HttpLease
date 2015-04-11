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
            GlobalConfig.Config.Host = "http://192.168.0.22:5698";
            _Http = HttpLease.Get<IHttp>();
        }

        [TestMethod]
        public void Get_Api_Test()
        {
            var result = _Http.Test(1, "234");
            Assert.AreEqual("{\"id\":1,\"value\":\"234\"}", result);

            result = _Http.Test4("234");
            Assert.AreEqual("{\"value2\":\"234\"}", result);
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

        [TestMethod]
        public void Get_Api_Test_With_Chinese()
        {
            var result = _Http.Test(3, "中国");
            Assert.AreEqual("{\"id\":3,\"value\":\"中国\"}", result);
        }

        [TestMethod]
        public void Get_Api_Test_With_Encode()
        {
            var result = _Http.Test(3, "http://www.baidu.com");
            Assert.AreEqual("{\"id\":3,\"value\":\"http://www.baidu.com\"}", result);
        }

        [TestMethod]
        public void Get_Api_Test_With_Chinese2()
        {
            var result = _Http.Test5(3, "中国");
            Assert.AreEqual("{\"id\":3,\"value\":\"中国\"}", result);
        }

        [TestMethod]
        public void Get_Api_Test_With_Encode2()
        {
            var result = _Http.Test5(3, "http://www.baidu.com");
            Assert.AreEqual("{\"id\":3,\"value\":\"http://www.baidu.com\"}", result);
        }


        [TestMethod]
        public void Post_Api_Test_With_Body()
        {
            var result = _Http.Post(3, "23456");
            Assert.AreEqual("{\"post\":3,\"value\":\"23456\"}", result);

            result = _Http.PostBody(3, "=234567");
            Assert.AreEqual("{\"post\":3,\"value\":\"234567\"}", result);
        }

        [TestMethod]
        public void Post_Api_Test_With_Path()
        {
            var result = _Http.Post2(4, "23456");
            Assert.AreEqual("{\"post2\":4,\"value\":\"23456\",\"value2\":null}", result);
        }

        [TestMethod]
        public void Post_Api_Test_With_Path_And_Body()
        {
            var result = _Http.Post3(4, "23456", "678");
            Assert.AreEqual("{\"post2\":4,\"value\":\"23456\",\"value2\":\"678\"}", result);
        }


        [TestMethod]
        public void Post_Api_Test_With_Object()
        {
            var result = _Http.Post4(5, new PostRequest { v1 = 3, v2 = DateTime.MaxValue }, new PostRequest2 { v3 = "hah", v4  = 1.02M });
            Assert.AreEqual("{\"post\":5,\"value\":{\"v1\":3,\"v2\":\"" + DateTime.MaxValue.ToString("yyyy-MM-ddTHH:mm:ss") + "\",\"v3\":\"hah\",\"v4\":1.02}}", result);
        }

        [TestMethod]
        public void Post_File()
        {
            try
            {
                var content = "HttpLease测试发送文件";
                using (var file = new System.IO.FileStream("1.txt", System.IO.FileMode.Create))
                {
                    var data = System.Text.Encoding.UTF8.GetBytes(content);
                    file.Write(data, 0, data.Length);
                }
                var result = _Http.PostFile(@"1.txt");
                Assert.AreEqual("{\"content\":\"" + content + "\",\"filename\":\"1.txt\"}", result);
            }
            finally
            {
                System.IO.File.Delete("1.txt");
            }        
        }

        [TestMethod]
        public void Post_File_With_Data()
        {
            try
            {
                var content = "HttpLease测试发送文件";
                using (var file = new System.IO.FileStream("1.txt", System.IO.FileMode.Create))
                {
                    var data = System.Text.Encoding.UTF8.GetBytes(content);
                    file.Write(data, 0, data.Length);
                }
                var result = _Http.PostFile2(@"1.txt", "test1");
                Assert.AreEqual("{\"content\":\"" + content + "\",\"filename\":\"1.txt\",\"strs\":\"test1\"}", result);
            }
            finally
            {
                System.IO.File.Delete("1.txt");
            }
        }

        [TestMethod]
        public void Delete_Api_Test_With_Body()
        {
            var result = _Http.Delete(3, "23456");
            Assert.AreEqual("{\"delete\":3,\"value\":\"23456\"}", result);
        }

        [TestMethod]
        public void Delete_Api_Test_With_Path()
        {
            var result = _Http.Delete2(4, "23456");
            Assert.AreEqual("{\"delete2\":4,\"value\":\"23456\",\"value2\":null}", result);
        }

        [TestMethod]
        public void Delete_Api_Test_With_Path_And_Body()
        {
            var result = _Http.Delete3(4, "23456", "678");
            Assert.AreEqual("{\"delete2\":4,\"value\":\"23456\",\"value2\":\"678\"}", result);
        }

        [TestMethod]
        public void Put_Api_Test_With_Body()
        {
            var result = _Http.Put(3, "23456");
            Assert.AreEqual("{\"put\":3,\"value\":\"23456\"}", result);
        }

        [TestMethod]
        public void Put_Api_Test_With_Path()
        {
            var result = _Http.Put2(4, "23456");
            Assert.AreEqual("{\"put2\":4,\"value\":\"23456\",\"value2\":null}", result);
        }

        [TestMethod]
        public void Put_Api_Test_With_Path_And_Body()
        {
            var result = _Http.Put3(4, "23456", "678");
            Assert.AreEqual("{\"put2\":4,\"value\":\"23456\",\"value2\":\"678\"}", result);
        }
    }
}
