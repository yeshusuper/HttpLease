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
                Formatter = new Formatters.FormFormatter()
            };
        }
    }
}
