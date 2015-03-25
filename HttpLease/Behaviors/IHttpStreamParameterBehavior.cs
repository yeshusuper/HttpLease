using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HttpLease.Behaviors
{
    internal interface IHttpStreamParameterBehavior : IHttpParameterBehavior
    {
        void CopyTo(System.IO.Stream stream, object[] args, ref bool firstPart, byte[] boundaryBytes, byte[] enterBytes);
    }

    internal class MultiPartParameters : List<IHttpStreamParameterBehavior>
    {
        public Encoding Encoding { get; private set; }

        public MultiPartParameters(Encoding encoding)
        {
            this.Encoding = encoding;
        }

        public void CopyTo(System.IO.Stream stream, object[] args, string boundary)
        {
            var boundaryBytes = Encoding.GetBytes(boundary);
            var enterBytes = Encoding.GetBytes("\r\n");
            var firstPart = true;
            foreach (var part in this)
            {
                part.CopyTo(stream, args, ref firstPart, boundaryBytes, enterBytes);
            }
        }
    }

    internal class HttpFileParameterBehavior : IHttpStreamParameterBehavior
    {
        public string Key { get; private set; }
        public int ArgIndex { get; private set; }
        public Encoding Encoding { get; private set; }
        public HttpFileParameterBehavior(string key, int argIndex, Encoding encoding)
        {
            this.Key = key;
            this.ArgIndex = argIndex;
            this.Encoding = encoding;
        }

        public void CopyTo(System.IO.Stream stream, object[] args, ref bool firstPart, byte[] boundaryBytes, byte[] enterBytes)
        {
            var arg = args[ArgIndex];
            if (arg == null) return;
            var contentType = "application/octet-stream";
            var filename = String.Empty;
            FileStream fs = null;
            if (arg is String)
                fs = File.OpenRead((String)arg);
            if (fs == null)
                fs = arg as FileStream;
            Stream source = null;
            if(fs != null)
            {
                filename = Path.GetFileName(fs.Name);
                contentType = Utilities.MimeTypeHelper.GetMimeType(filename);
                source = fs;
            }
            else
            {
                if (arg is System.Web.HttpPostedFile)
                {
                    var pf = arg as System.Web.HttpPostedFile;
                    filename = pf.FileName;
                    contentType = Utilities.MimeTypeHelper.GetMimeType(filename);
                    source = pf.InputStream;
                }
                else if (arg is System.Web.HttpPostedFileBase)
                {
                    var pf = arg as System.Web.HttpPostedFileBase;
                    filename = pf.FileName;
                    contentType = Utilities.MimeTypeHelper.GetMimeType(filename);
                    source = pf.InputStream;
                }
                else
                {
                    source = arg as Stream;
                }
            }
            if (source == null) return;
            using (source)
            {
                if (!firstPart)
                    stream.Write(enterBytes, 0, enterBytes.Length);
                else
                    firstPart = false;
                stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                stream.Write(enterBytes, 0, enterBytes.Length);
                var dispositionBytes = Encoding.GetBytes(String.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n", Key, filename));
                stream.Write(dispositionBytes, 0, dispositionBytes.Length);
                var contentTypeBytes = Encoding.GetBytes("Content-Type: " + contentType + "\r\n\r\n");
                stream.Write(contentTypeBytes, 0, contentTypeBytes.Length);
                var buffer = new byte[1024];
                var rl = 0;
                while ((rl = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, rl);
                }
            }
        }
    }

    internal class HttpStringParameterStreamWrapperBehavior : IHttpStreamParameterBehavior
    {
        private IHttpStringParameterBehavior _StringParameterBehavior;

        public HttpStringParameterStreamWrapperBehavior(IHttpStringParameterBehavior stringParameterBehavior)
        {
            this._StringParameterBehavior = stringParameterBehavior;
        }

        public void CopyTo(System.IO.Stream stream, object[] args, ref bool firstPart, byte[] boundaryBytes, byte[] enterBytes)
        {
            var encoding = _StringParameterBehavior.Encoding;
            var rps = _StringParameterBehavior.GetRequestParameters(args);
            foreach (var item in rps)
	        {
                var dispositionBytes = encoding.GetBytes(String.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n", item.Key));
		        foreach (var value in item.Value)
                {
                    if (!firstPart)
                        stream.Write(enterBytes, 0, enterBytes.Length);
                    else
                        firstPart = false;
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    stream.Write(enterBytes, 0, enterBytes.Length);
                    stream.Write(dispositionBytes, 0, dispositionBytes.Length);
                    if (!String.IsNullOrEmpty(value))
                    {
                        var data = encoding.GetBytes(value);
                        stream.Write(data, 0, data.Length);
                    }
	            }
	        }

        }

        public string Key
        {
            get { return _StringParameterBehavior.Key; }
        }

        public int ArgIndex
        {
            get { return _StringParameterBehavior.ArgIndex; }
        }
    }
}
