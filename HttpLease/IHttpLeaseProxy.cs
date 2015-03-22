using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    internal interface IHttpLeaseProxy<T>
        where T : class
    {
        T Client { get; }
    }

    internal class HttpLeaseProxy<T> : IHttpLeaseProxy<T>
        where T : class
    {
        private class Interceptor : Castle.DynamicProxy.IInterceptor
        {
            private readonly Behaviors.IHttpBehavior[] _Behaviors;

            public Interceptor(Behaviors.IHttpBehavior[] behaviors)
            {
                _Behaviors = behaviors ?? new Behaviors.IHttpBehavior[0];
            }

            public void Intercept(Castle.DynamicProxy.IInvocation invocation)
            {
                var behavoir = _Behaviors.First(b => b.IsMatch(invocation));
                
            }
        }



        private static Dictionary<Type, Behaviors.IHttpBehavior[]> _BehaviorsCache = new Dictionary<Type, Behaviors.IHttpBehavior[]>();
        private static Castle.DynamicProxy.ProxyGenerator _ProxyGenerator = new Castle.DynamicProxy.ProxyGenerator();

        public IConfig Config { get; private set; }
        private T _Instance = null;

        public T Client
        {
            get { return this._Instance ?? CreateInstance(); }
        }

        private T CreateInstance()
        {
            var interceptor = new Interceptor(GetBehaviorsWithCache(typeof(T), Config));
            return this._Instance = (T)_ProxyGenerator.CreateInterfaceProxyWithoutTarget(typeof(T), interceptor);
        }

        private IEnumerable<Behaviors.IHttpBehavior> GetBehaviors(Type type, IConfig config)
        {
            if (!type.IsInterface)
                throw new Exception("只支持接口类型");

            var factory = new Behaviors.HttpBehaviorFactory();
            
            foreach (var item in type.GetMethods())
	        {
                yield return factory.Create(item, config);
	        }
        }

        private Behaviors.IHttpBehavior[] GetBehaviorsWithCache(Type type, IConfig config)
        {
            Behaviors.IHttpBehavior[] result;
            if (!_BehaviorsCache.TryGetValue(type, out result))
            {
                _BehaviorsCache[type] = result = GetBehaviors(type, config).ToArray();
            }
            return result;
        }
    }
}
