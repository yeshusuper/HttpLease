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

        [Timeout(5000)]
        [HttpGet]
        [Url("http://apis.baidu.com/apistore/iplookupservice/iplookup")]
        BaiduIPResponse BaiduIP([Header(Name = "apikey")]string apiKey, string ip);
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

    public class BaiduIPResponse
    {
        public class Ret
        {
            [JsonProperty(PropertyName = "ip")]
            public string IP { get; set; }
            [JsonProperty(PropertyName = "country")]
            public string Country { get; set; }
            [JsonProperty(PropertyName = "province")]
            public string Province { get; set; }
            [JsonProperty(PropertyName = "city")]
            public string City { get; set; }
            [JsonProperty(PropertyName = "district")]
            public string District { get; set; }
            [JsonProperty(PropertyName = "carrier")]
            public string Carrier { get; set; }

        }

        [JsonProperty(PropertyName = "errNum")]
        public int ErrNum { get; set; }
        [JsonProperty(PropertyName = "errMsg")]
        public string ErrMsg { get; set; }
        [JsonProperty(PropertyName = "retData")]
        public Ret RetData { get; set; }
    }
}
