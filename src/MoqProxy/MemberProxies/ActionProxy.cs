using Moq;
using MoqProxy.Extensions;
using MoqProxy.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MoqProxy.MemberProxies
{
    public class ActionProxy<T> : IMemberProxy<T>
        where T : class
    {
        private readonly Dictionary<Type[], MethodInfo> _genericMethodInfos = new(TypeArrayEqualityComparer.Instance);

        public MethodInfo MethodInfo { get; }
        public Expression<Action<T>> MockExpresison { get; }

        private ActionProxy(MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
            this.MockExpresison = methodInfo.GetMockActionExpression<T>();
        }

        public void Setup(Mock<T> mock, T proxy)
        {
            mock.Setup(this.MockExpresison).Callback(new InvocationAction(invocation =>
            {
                Type[] genericArguments = invocation.Method.GetGenericArguments();
                MethodInfo methodInfo = this.GetMethodInfo(genericArguments);
                methodInfo.Invoke(proxy, [.. invocation.Arguments]);
            }));
        }

        private MethodInfo GetMethodInfo(Type[] genericArguments)
        {
            if (genericArguments.Length == 0 || this.MethodInfo.IsGenericMethodDefinition == false)
            {
                return this.MethodInfo;
            }

            ref MethodInfo? methodInfo = ref CollectionsMarshal.GetValueRefOrAddDefault(this._genericMethodInfos, genericArguments, out bool exists);
            if (exists == true)
            {
                return methodInfo!;
            }

            methodInfo = this.MethodInfo.MakeGenericMethod(genericArguments);
            return methodInfo;
        }

        private static readonly Dictionary<MethodInfo, ActionProxy<T>> _cache = [];
        public static ActionProxy<T> Get(MethodInfo methodInfo)
        {
            ref ActionProxy<T>? proxy = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, methodInfo, out bool exists);
            if (exists == true)
            {
                return proxy!;
            }

            proxy = new ActionProxy<T>(methodInfo);

            return proxy;
        }
    }
}
