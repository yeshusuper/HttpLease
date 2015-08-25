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
        private static Regex _UrlPathRegex = new Regex("\\{([^\\}]+)\\}", RegexOptions.Compiled);

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

            var timeout = methodAttrs.FirstOrDefault(a => a is TimeoutAttribute) as TimeoutAttribute;
            if (timeout != null)
                behavior.Timeout = timeout.Timeout;

            var url = String.Empty;
            var urlAttr = methodAttrs.FirstOrDefault(a => a is UrlAttribute) as UrlAttribute;
            if (urlAttr != null && urlAttr.Url != null)
                url = urlAttr.Url.Trim();

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
                if (!behavior.Url.ToLower().StartsWith("http"))
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
                var parmeterAttr = param.GetCustomAttributes(typeof(ParameterAttribute), false).FirstOrDefault() as ParameterAttribute;
                if (parmeterAttr == null || parmeterAttr is PathAttribute)
                {
                    paramName = GetParmeterName(parmeterAttr) ?? paramName;
                    if (behavior.IsWithPath && urlPathIndexs.Values.Contains(paramName))
                    {
                        foreach (var pair in urlPathIndexs)
                        {
                            if (pair.Value == paramName)
                            {
                                behavior.PathKeys.Add(pair.Key, new HttpParameterBehavior(paramName, i, config.Encoding, config.Formatter));
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

                paramName = GetParmeterName(parmeterAttr) ?? paramName;

                var add = false;
                add = add || ParameterBehavior(behavior, paramName, parmeterAttr as HeaderAttribute, i, config.Encoding, config.Formatter);
                add = add || ParameterBehavior(behavior, paramName, parmeterAttr as FieldAttribute, enctypeAttr, i, config.Encoding, config.Formatter);
                add = add || ParameterBehavior(behavior, paramName, parmeterAttr as FieldMapAttribute, enctypeAttr, i, config.Encoding, config.Formatter);
                add = add || ParameterBehavior(behavior, paramName, parmeterAttr as PartAttribute, enctypeAttr, i, config.Encoding, config.Formatter);
                add = add || ParameterBehavior(behavior, paramName, parmeterAttr as QueryAttribute, enctypeAttr, i, config.Encoding, config.Formatter);
                add = add || ParameterBehavior(behavior, paramName, parmeterAttr as BodyAttribute, i);
            }

            behavior.Verify();

            return behavior;
        }

        private string GetParmeterName(ParameterAttribute parmeterAttr)
        {
            if (parmeterAttr != null && parmeterAttr is BaseParameterAttribute)
            {
                var name = ((BaseParameterAttribute)parmeterAttr).Name;
                if (name != null)
                {
                    return name.Trim().ToLower();
                }
            }
            return null;
        }

        private bool ParameterBehavior(IHttpBehavior behavior, string paramName, BodyAttribute attr, int argIndex)
        {
            if (attr == null) return false;
            if (behavior.BodyKey != null)
                throw new Exception("只能设置一次body");
            behavior.BodyKey = new HttpBodyBehavior(argIndex);
            return true;
        }

        private bool ParameterBehavior(IHttpBehavior behavior, string paramName, HeaderAttribute attr, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            if (attr == null) return false;
            behavior.HeaderKeys.Add(new HttpParameterBehavior(paramName, argIndex, encoding, formatter));
            return true;
        }

        private bool ParameterBehavior(IHttpBehavior behavior, string paramName, QueryAttribute attr, EnctypeAttribute enctype, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            if (attr == null) return false;
            behavior.QueryKeys.Add(new HttpParameterBehavior(paramName, argIndex, encoding, formatter)
            {
                IsEncodeKey = attr.IsEncodeKey ?? enctype.DefaultEncodeKey,
                IsEncodeValue = true
            });
            return true;
        }

        private bool ParameterBehavior(IHttpBehavior behavior, string paramName, FieldAttribute attr, EnctypeAttribute enctype, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            if (attr == null) return false;
            var pb = new HttpParameterBehavior(paramName, argIndex, encoding, formatter)
            {
                IsEncodeKey = attr.IsEncodeKey ?? enctype.DefaultEncodeKey,
                IsEncodeValue = attr.IsEncodeValue ?? enctype.DefaultEncodeValue
            };
            if (enctype is MultipartAttribute)
            {
                behavior.PartKeys.Add(new HttpStringParameterStreamWrapperBehavior(pb));
            }
            else
            {
                behavior.FieldKeys.Add(pb);
            }
            return true;
        }

        private bool ParameterBehavior(IHttpBehavior behavior, string paramName, FieldMapAttribute attr, EnctypeAttribute enctype, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            if (attr == null) return false;
            behavior.FieldKeys.Add(new FieldMapParameterBehavior(paramName, argIndex, encoding, formatter)
            {
                IsEncodeKey = attr.IsEncodeKey ?? enctype.DefaultEncodeKey,
                IsEncodeValue = attr.IsEncodeValue ?? enctype.DefaultEncodeValue
            });
            return true;
        }

        private bool ParameterBehavior(IHttpBehavior behavior, string paramName, PartAttribute attr, EnctypeAttribute enctype, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            if (attr == null) return false;
            if (!attr.IsFile)
            {
                var pb = new HttpParameterBehavior(paramName, argIndex, encoding, formatter)
                {
                    IsEncodeKey = enctype.DefaultEncodeKey,
                    IsEncodeValue = enctype.DefaultEncodeValue
                };
                behavior.PartKeys.Add(new HttpStringParameterStreamWrapperBehavior(pb));
            }
            else
            {
                behavior.PartKeys.Add(new HttpFileParameterBehavior(paramName, argIndex, encoding));
            }
            return true;
        }
    }
}
