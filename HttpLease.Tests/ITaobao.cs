using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpLease.Tests
{
    interface ITaobao
    {
        [CustomEnctype("")]
        [Timeout(5000)]
        [HttpGet]
        [Url("http://ip.taobao.com/service/getIpInfo.php")]
        TaoBaoIPResponse Get(string ip);
    }
    public class TaoBaoIPResponse
    {
        public class IpInfo
        {
            [JsonProperty(PropertyName = "ip")]
            public string IP { get; set; }
            [JsonProperty(PropertyName = "area")]
            public string Region { get; set; }
            [JsonProperty(PropertyName = "city")]
            public string City { get; set; }
            [JsonProperty(PropertyName = "isp")]
            public string Isp { get; set; }
        }
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "data")]
        public IpInfo Data { get; set; }
    }
}
