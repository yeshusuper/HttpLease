using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Tests
{
    public class PostRequest
    {
        public long v1 { get; set; }
        public DateTime v2 { get; set; }
    }
    public class PostRequest2
    {
        public string v3 { get; set; }
        public decimal v4 { get; set; }
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
}
