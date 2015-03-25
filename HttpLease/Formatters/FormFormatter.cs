using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Formatters
{
    public class FormFormatter : IFormatter
    {
        public RequestParameters GetRequestParameters(string key, object value, Encoding encoding)
        {
            Dictionary<string, string[]> result = null;

            if (value != null)
            {
                var jt = JConstructor.FromObject(value);
                result = GetParameters(key, jt);
            }
            if (result == null)
                result = new Dictionary<string, string[]>();

            return new RequestParameters(result, encoding);
        }

        public RequestParameters GetRequestParameters(object value, Encoding encoding)
        {
            Dictionary<string, string[]> result = null;

            if (value != null)
            {
                var jt = JConstructor.FromObject(value) as JObject;
                if (jt == null)
                    throw new Exception("只支持能转换为json的object对象的类型");
                result = new Dictionary<string, string[]>();
                foreach (var item in jt)
                {
                    AddToDictionary(result, GetParameters(item.Key, item.Value));
                }
            }
            if (result == null)
                result = new Dictionary<string, string[]>();

            return new RequestParameters(result, encoding);
        }



        private Dictionary<string, string[]> GetParameters(string prefix, JToken jt)
        {
            if (jt is JObject)
            {
                return GetObjectParameters(prefix, jt as JObject);
            }
            else if (jt is JArray)
            {
                return GetArrayParameters(prefix, jt as JArray);
            }
            else
            {
                return GetValueParameters(prefix, jt);
            }
        }

        private Dictionary<string, string[]> GetObjectParameters(string prefix, JObject jt)
        {
            var result = new Dictionary<string, string[]>();
            foreach (var item in jt)
            {
                AddToDictionary(result, GetParameters(String.Format("{0}[{1}]", prefix, item.Key), item.Value));
            }
            return result;
        }

        private Dictionary<string, string[]> GetArrayParameters(string prefix, JArray jt)
        {
            var result = new Dictionary<string, string[]>();
            var addIndex = jt.Any(item => item.Type == JTokenType.Array || item.Type == JTokenType.Object);
            for (int i = 0; i < jt.Count; i++)
            {
                var item = jt[i];
                AddToDictionary(result, GetParameters(String.Format("{0}[{1}]", prefix, addIndex ? i.ToString() : String.Empty), item));
            }
            return result;
        }

        private Dictionary<string, string[]> GetValueParameters(string prefix, JToken jt)
        {
            return new Dictionary<string, string[]>() { { prefix, new string[] { jt.ToString() } } };
        }

        private void AddToDictionary(Dictionary<string, string[]> first, Dictionary<string, string[]> second)
        {
            foreach (var item in second)
            {
                AddToDictionary(first, item.Key, item.Value);
            }
        }

        private void AddToDictionary(Dictionary<string, string[]> dic, string key, string[] value)
        {
            string[] result;
            if (!dic.TryGetValue(key, out result))
                dic[key] = value;
            else
            {
                dic[key] = result.Concat(value).ToArray();
            }
        }
    }
}
