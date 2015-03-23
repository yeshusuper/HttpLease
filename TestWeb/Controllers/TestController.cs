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
    }
}
