using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestWeb.Controllers
{
    public class TestController : ApiController
    {
        public object Get(long id, string value)
        {
            return new
            {
                id = id,
                value = value
            };
        }
        public object Get(string value2)
        {
            return new
            {
                value2 = value2
            };
        }

        public object Post(long post, [FromBody]string value)
        {
            return new
            {
                post = post,
                value = value
            };
        }

        [HttpPost]
        public object Post2([FromUri(Name = "id")]long post2, string value, [FromBody]string value2)
        {
            return new
            {
                post2 = post2,
                value = value,
                value2 = value2
            };
        }

        public object Delete(long delete, [FromBody]string value)
        {
            return new
            {
                delete = delete,
                value = value
            };
        }

        [HttpDelete]
        public object Delete2([FromUri(Name = "id")]long delete2, string value, [FromBody]string value2)
        {
            return new
            {
                delete2 = delete2,
                value = value,
                value2 = value2
            };
        }

        public object Put(long put, [FromBody]string value)
        {
            return new
            {
                put = put,
                value = value
            };
        }

        [HttpPut]
        public object Put2([FromUri(Name = "id")]long put2, string value, [FromBody]string value2)
        {
            return new
            {
                put2 = put2,
                value = value,
                value2 = value2
            };
        }
    }
}
