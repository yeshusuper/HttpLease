using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Utilities
{
    public static class Headers
    {
        public const string ContentType = "Content-Type";
        public const string CacheControl = "Cache-Control";
        public const string Accept = "Accept";
        public const string AcceptLanguage = "Accept-Language";
        public const string AcceptEncoding = "Accept-Encoding";
        public const string Origin = "Origin";

        public static string CreateBoundary()
        {
            using(var md5 = System.Security.Cryptography.MD5.Create())
            {
                var bas = md5.ComputeHash(Guid.NewGuid().ToByteArray());

                var sBuilder = new StringBuilder();
                for (int i = 0; i < bas.Length; i++)
                {
                    sBuilder.Append(bas[i].ToString("x2"));
                }
                var result = sBuilder.ToString();
                return result.PadLeft(36, '-');
            }
        }
    }
}
