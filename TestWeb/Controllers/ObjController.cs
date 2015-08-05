using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TestWeb.Controllers
{
    public class ObjController : ApiController
    {
        public class PostRequest
        {
            public long v1 { get; set; }
            public DateTime v2 { get; set; }
            public string v3 { get; set; }
            public decimal v4 { get; set; }
        }

        public object Post([FromUri]long id, [FromBody]PostRequest request)
        {
            return new {
                post = id,
                value = request,
            };
        }

        public class PutRequest
        {
            public C[] A { get; set; }
            public string B { get; set; }

            public class C
            {
                public string D { get; set; }
            }
        }

        [HttpPut]
        public object Put([FromBody]PutRequest request)
        {
            return request;
        }
    }
}
