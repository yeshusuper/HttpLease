using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FormUrlEncodedAttribute : EnctypeAttribute
    {
        /// <summary>
        /// 默认为true
        /// </summary>
        public bool IsEncodeKey { get; set; }
        /// <summary>
        /// 默认为true
        /// </summary>
        public bool IsEncodeValue { get; set; }

        public FormUrlEncodedAttribute()
        {
            IsEncodeKey = true;
            IsEncodeValue = true;
        }

        internal override string ContentType
        {
            get { return "application/x-www-form-urlencoded"; }
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
