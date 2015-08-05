using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class EnctypeAttribute : Attribute
    {
        internal abstract string ContentType { get; }
        internal abstract bool DefaultEncodeKey { get; }
        internal abstract bool DefaultEncodeValue { get; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomEnctypeAttribute : EnctypeAttribute
    {
        /// <summary>
        /// 默认为true
        /// </summary>
        public bool IsEncodeKey { get; set; }
        /// <summary>
        /// 默认为true
        /// </summary>
        public bool IsEncodeValue { get; set; }

        public CustomEnctypeAttribute(string contentType)
        {
            IsEncodeKey = true;
            IsEncodeValue = true;
            _ContentType = contentType;
        }

        private string _ContentType;

        internal override string ContentType
        {
            get { return _ContentType; }
        }

        internal override bool DefaultEncodeKey
        {
            get { return IsEncodeKey; }
        }

        internal override bool DefaultEncodeValue
        {
            get { return IsEncodeValue; }
        }
    }
}
