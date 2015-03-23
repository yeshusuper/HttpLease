using HttpLease.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace HttpLease.Behaviors
{
    internal interface IHttpBehavior
    {
        /// <summary>
        /// 包含的head参数
        /// </summary>
        List<IHttpParameterBehavior> HeaderKeys { get; }
        /// <summary>
        /// 包含的Path参数
        /// </summary>
        IDictionary<int, IHttpParameterBehavior> PathKeys { get; }
        /// <summary>
        /// 包含的Query参数
        /// </summary>
        List<IHttpParameterBehavior> QueryKeys { get; }
        /// <summary>
        /// 包含的Field参数
        /// </summary>
        List<IHttpParameterBehavior> FieldKeys { get; }
        string Host { get; }
        MethodKind Method { get; }
        /// <summary>
        /// 网络地址
        /// </summary>
        string Url { get; }
        /// <summary>
        /// url中是否包含参数
        /// </summary>
        bool IsWithPath { get; }
        Encoding Encoding { get; }
        Formatters.IFormatter Formatter { get; }
        IDictionary<string, string> FiexdHeaders { get; }
        Type ReturnType { get; }

        bool IsMatch(Castle.DynamicProxy.IInvocation invocation);
        void Verify();

        HttpWebRequest CreateHttpWebRequest(object[] args);
    }

    internal class HttpBehavior : IHttpBehavior
    {
        private class ParameterInfoMatcher
        {
            private readonly bool _IsNullable;
            private readonly Type _ParameterType;

            public ParameterInfoMatcher(ParameterInfo info)
            {
                _ParameterType = info.ParameterType;
                _IsNullable = info.ParameterType.IsGenericType &&
                    info.ParameterType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            public bool IsMatch(object obj)
            {
                if (_ParameterType.IsValueType && !_IsNullable && obj == null)
                    return false;

                return obj == null ||
                            _ParameterType.IsAssignableFrom(obj.GetType());
            }
        }

        private readonly MethodInfo _MethodInfo;
        private readonly ParameterInfoMatcher[] _ParameterInfos;

        public HttpBehavior(IConfig config, MethodInfo methodInfo)
        {
            _MethodInfo = methodInfo;
            _ParameterInfos = methodInfo.GetParameters().Select(p => new ParameterInfoMatcher(p)).ToArray();
            HeaderKeys = new List<IHttpParameterBehavior>();
            PathKeys = new Dictionary<int, IHttpParameterBehavior>();
            QueryKeys = new List<IHttpParameterBehavior>();
            FieldKeys = new List<IHttpParameterBehavior>();
            FiexdHeaders = new Dictionary<string, string>(config.FiexdHeaders);
            Encoding = config.Encoding;
            Host = config.Host;
            Formatter = config.Formatter;
            ReturnType = methodInfo.ReturnType;
        }

        public List<IHttpParameterBehavior> HeaderKeys { get; private set; }
        public IDictionary<int, IHttpParameterBehavior> PathKeys { get; private set; }
        public List<IHttpParameterBehavior> QueryKeys { get; private set; }
        public List<IHttpParameterBehavior> FieldKeys { get; private set; }
        public string Url { get; set; }
        public bool IsWithPath { get; set; }
        public Encoding Encoding { get; set; }
        public MethodKind Method { get; set; }
        public string Host { get; private set; }
        public Formatters.IFormatter Formatter { get; set; }
        public Type ReturnType { get; set; }
        public IDictionary<string, string> FiexdHeaders { get; private set; }

        public bool IsMatch(Castle.DynamicProxy.IInvocation invocation)
        {
            if (_MethodInfo != invocation.Method)
                return false;
            if (_ParameterInfos.Length != invocation.Arguments.Length)
                return false;
            for (int i = 0; i < _ParameterInfos.Length; i++)
            {
                if (!_ParameterInfos[i].IsMatch(invocation.Arguments[i]))
                    return false;
            }
            return true;
        }

        public HttpWebRequest CreateHttpWebRequest(object[] args)
        {
            var url = Url;
            if(IsWithPath && PathKeys.Count > 0)
            {
                var strs = new string[PathKeys.Count];
                foreach (var item in PathKeys)
                {
                    strs[item.Key] = args[item.Value.ArgIndex].ToString();
                }
                url = String.Format(url, strs);
            }

            var querys = new List<string>();
            foreach (var item in QueryKeys)
            {
                var rps = Formatter.GetRequestParameters(item.Key, args[item.ArgIndex], Encoding);
                querys.Add(rps.ToString(item.IsEncodeKey, item.IsEncodeValue));
            }
            if(querys.Count > 0)
            {
                url += (url.IndexOf('?') == -1 ? "?" : "&") + String.Join("&", querys);
            }

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = Method.ToString();
            request.ContentType = FiexdHeaders[Headers.ContentType];
            if (Host != null)
            {
                var uri = new Uri(Host);
                request.Host = uri.Host;
            }

            foreach (var item in HeaderKeys)
            {
                request.Headers[item.Key] = args[item.ArgIndex].ToString();
            }

            var fields = new List<string>();
            foreach (var item in FieldKeys)
            {
                var rps = Formatter.GetRequestParameters(item.Key, args[item.ArgIndex], Encoding);
                fields.Add(rps.ToString(item.IsEncodeKey, item.IsEncodeValue));
            }

            if(MethodKind.GET != Method && fields.Count > 0)
            {
                var fieldContent = String.Join("&", fields);
                if (Method == MethodKind.PUT || Method == MethodKind.DELETE)
                    request.ContentLength = fieldContent.Length;

                var dataWriter = request.GetRequestStream();
                byte[] d = Encoding.GetBytes(fieldContent);
                dataWriter.Write(d, 0, d.Length);
                dataWriter.Flush();
            }

            return request;
        }

        public void Verify()
        {
            if (MethodKind.GET == Method && FieldKeys.Count > 0)
                throw new Exception("get 情况不能使用 Field");

        }
    }

}
