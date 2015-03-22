using HttpLease.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpLease.Behaviors
{
    internal interface IHttpBehaviorFactory
    {
        IHttpBehavior Create(MethodInfo methodInfo);
    }

    internal class HttpBehaviorFactory
    {
        private static Regex _UrlPathRegex = new Regex("\\{(^\\})+\\}", RegexOptions.Compiled);

        public IHttpBehavior Create(MethodInfo methodInfo, IConfig config)
        {
            if (methodInfo == null) return null;

            var behavior = new HttpBehavior(config, methodInfo);

            var methodAttrs = methodInfo.GetCustomAttributes(false);

            var methodAttr = methodAttrs.FirstOrDefault(a => a is HttpMethodAttribute) as HttpMethodAttribute;
            if (methodAttr == null)
                methodAttr = new HttpGetAttribute();
            behavior.Method = methodAttr.Method;

            var enctypeAttr = methodAttrs.FirstOrDefault(a => a is EnctypeAttribute) as EnctypeAttribute;
            if (enctypeAttr == null)
            {
                enctypeAttr = new FormUrlEncodedAttribute();
                if (!behavior.FiexdHeaders.ContainsKey(Headers.ContentType))
                    behavior.FiexdHeaders[Headers.ContentType] = enctypeAttr.ContentType;
            }
            else
            {
                behavior.FiexdHeaders[Headers.ContentType] = enctypeAttr.ContentType;
            }

            var url = String.Empty;
            var urlAttr = methodAttrs.FirstOrDefault(a => a is UrlAttribute) as UrlAttribute;
            if (urlAttr != null && urlAttr.Url != null)
                url = urlAttr.Url.ToLower().Trim();

            var urlPathIndexs = new Dictionary<int, string>(); 

            if (!String.IsNullOrEmpty(url))
            {
                int index = 0;
                behavior.Url = _UrlPathRegex.Replace(url, new MatchEvaluator(match =>
                {
                    var r = index++;
                    urlPathIndexs.Add(r, match.Groups[1].Value.ToLower().Trim());
                    return "{" + r + "}";
                }));
                behavior.IsWithPath = behavior.Url != url;
                if (!behavior.Url.StartsWith("http"))
                {
                    if (!behavior.Url.StartsWith("/"))
                        behavior.Url = "/" + behavior.Url;
                    behavior.Url = config.Host + behavior.Url;
                }
            }
            else
            {
                behavior.Url = config.Host;
            }

            var parameters = methodInfo.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var paramName = param.Name.ToLower();
                var parmeterAttr = param.GetCustomAttributes(typeof(BaseParameterAttribute), false).FirstOrDefault() as BaseParameterAttribute;
                if (parmeterAttr == null || parmeterAttr is PathAttribute)
                {
                    if (parmeterAttr != null && parmeterAttr.Key != null)
                        paramName = parmeterAttr.Key.Trim().ToLower();
                    if (behavior.IsWithPath && urlPathIndexs.Values.Contains(paramName))
                    {
                        foreach (var pair in urlPathIndexs)
                        {
                            if (pair.Value == paramName)
                            {
                                behavior.PathKeys.Add(pair.Key, new HttpParameterBehavior(paramName, i));
                            }
                        }
                        continue;
                    }
                    else if (parmeterAttr == null)
                    {
                        parmeterAttr = new QueryAttribute();
                    }
                    else
                    {
                        throw new Exception("url中没有定义对应的path：" + paramName);
                    }
                }

                if (parmeterAttr.Key != null)
                    paramName = parmeterAttr.Key.Trim().ToLower();


                ParameterBehavior(behavior, paramName, parmeterAttr as HeaderAttribute, i);
                ParameterBehavior(behavior, paramName, parmeterAttr as QueryAttribute, enctypeAttr, i);
                ParameterBehavior(behavior, paramName, parmeterAttr as FieldAttribute, enctypeAttr, i);
            }

            behavior.Verify();

            return behavior;
        }

        private void ParameterBehavior(IHttpBehavior behavior, string paramName, HeaderAttribute attr, int argIndex)
        {
            if (attr == null) return;
            behavior.HeaderKeys.Add(new HttpParameterBehavior(paramName, argIndex));
        }

        private void ParameterBehavior(IHttpBehavior behavior, string paramName, QueryAttribute attr, EnctypeAttribute enctype, int argIndex)
        {
            if (attr == null) return;
            behavior.QueryKeys.Add(new HttpParameterBehavior(paramName, argIndex)
            {
                IsEncodeKey = attr.IsEncodeKey ?? enctype.DefaultEncodeKey,
                IsEncodeValue = attr.IsEncodeValue ?? enctype.DefaultEncodeValue
            });
        }

        private void ParameterBehavior(IHttpBehavior behavior, string paramName, FieldAttribute attr, EnctypeAttribute enctype, int argIndex)
        {
            if (attr == null) return;
            behavior.FieldKeys.Add(new HttpParameterBehavior(paramName, argIndex)
            {
                IsEncodeKey = attr.IsEncodeKey ?? enctype.DefaultEncodeKey,
                IsEncodeValue = attr.IsEncodeValue ?? enctype.DefaultEncodeValue
            });
        }
    }
}
