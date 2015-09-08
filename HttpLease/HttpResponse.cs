using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    internal class HttpResponse
    {
        private readonly System.Net.HttpWebResponse _Response;
        private readonly System.IO.Stream _Stream;
        private readonly Encoding _ResponseEncoding;

        internal HttpResponse(System.Net.HttpWebResponse response, Encoding responseEncoding)
        {
            _Response = response;
            _Stream = response.GetResponseStream();
            _ResponseEncoding = responseEncoding;
        }

        public bool TryConvert(Type returnType, out object result)
        {
            result = null;
            if (returnType == null)
                return true;
            if (returnType.IsAssignableFrom(typeof(System.Net.WebResponse)))
                result = _Response;
            else
            {
                Verify();
                if (returnType == typeof(String))
                {
                    result = ReadString();
                }
                else
                {
                    try
                    {
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject(ReadString(), returnType);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private string ReadString()
        {
            var encoding = _ResponseEncoding;
            if(encoding == null)
                try
                {
                    encoding = Encoding.GetEncoding(_Response.CharacterSet);
                }
                catch { }
            if (encoding == null)
                encoding = Encoding.Default;
            if (_Stream != null)
            {
                using(_Stream)
                {
                    using (var reader = new System.IO.StreamReader(_Stream, encoding))
                    {                    
                        return reader.ReadToEnd();
                    }
                }
            }
            else
            {
                return String.Empty;
            }
        }

        private void Verify()
        {
            if (_Response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = ReadString();
                throw new HttpResponseException(_Response.StatusCode, message);
            }
        }
    }
}
