using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Behaviors
{
    internal interface IHttpParameterBehavior
    {
        string Key { get; }
        int ArgIndex { get; }
    }
}
