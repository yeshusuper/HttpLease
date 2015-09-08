using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    public interface IConfig
    {
        Encoding Encoding { get; set; }
        Encoding ResponseEncoding { get; set; }
        IDictionary<string, string> FiexdHeaders { get; }
        string Host { get; set; }
        Formatters.IFormatter Formatter { get; set; }
        System.Net.CookieContainer CookieContainer { get; set; }
    }

    internal class Config : IConfig
    {
        public Encoding Encoding { get; set; }
        public Encoding ResponseEncoding { get; set; }
        public IDictionary<string, string> FiexdHeaders { get; private set; }

        private string _Host;
        public string Host 
        { 
            get { return _Host; } 
            set 
            {
                if (value != null && value.EndsWith("/")) _Host = value.Substring(0, _Host.Length - 1);
                else _Host = value;
            } 
        }
        public Formatters.IFormatter Formatter { get; set; }

        public Config()
        {
            FiexdHeaders = new Dictionary<string, string>();
        }

        public Config(IConfig config)
        {
            Encoding = config.Encoding;
            ResponseEncoding = config.ResponseEncoding;
            Host = config.Host;
            FiexdHeaders = new Dictionary<string, string>(config.FiexdHeaders);
            Formatter = config.Formatter;
        }

        public System.Net.CookieContainer CookieContainer { get; set; }
    }
}
