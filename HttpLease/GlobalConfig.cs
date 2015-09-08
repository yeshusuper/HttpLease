using HttpLease.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    public static class GlobalConfig
    {
        public static IConfig Config { get; private set; }

        static GlobalConfig()
        {
            Config = new Config
            {
                Encoding = Encoding.UTF8,
                DefaultResponseEncoding = Encoding.Default,
                Formatter = new Formatters.FormFormatter()
            };
            Config.FiexdHeaders[Headers.CacheControl] = " max-age=0";
            Config.FiexdHeaders[Headers.Accept] = " text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            Config.FiexdHeaders[Headers.AcceptLanguage] = " zh-CN,zh;q=0.8";
            Config.FiexdHeaders[Headers.AcceptEncoding] = "";
        }
    }
}
