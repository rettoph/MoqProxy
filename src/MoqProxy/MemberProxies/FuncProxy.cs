using Moq;
using MoqProxy.Extensions;
using MoqProxy.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MoqProxy.MemberProxies
{
    public abstract class FuncProxy<T> : IMemberProxy<T>
        where T : class
    {
        public abstract void Setup(Mock<T> mock, T proxy);

        private static readonly Dictionary<MethodInfo, FuncProxy<T>> _cache = [];
        public static FuncProxy<T> Get(MethodInfo methodInfo)
        {
            ref FuncProxy<T>? proxy = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, methodInfo, out bool exists);
            if (exists == true)
            {
                return proxy!;
            }

            Type returnType = methodInfo.ReturnType.IsGenericParameter
                ? typeof(It.IsAnyType)
                : methodInfo.ReturnType;

            Type funcProxyType = typeof(FuncProxy<,>).MakeGenericType(typeof(T), returnType);
            proxy = (FuncProxy<T>)(Activator.CreateInstance(funcProxyType, [methodInfo]) ?? throw new NotImplementedException());

            return proxy;
        }
    }
    public class FuncProxy<T, TOut> : FuncProxy<T>
        where T : class
    {
        private readonly Dictionary<Type[], MethodInfo> _genericMethodInfos = new(TypeArrayEqualityComparer.Instance);

        public MethodInfo MethodInfo { get; }
        public Expression<Func<T, TOut>> MockExpresison { get; }

        public FuncProxy(MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
            this.MockExpresison = methodInfo.GetMockFuncExpression<T, TOut>();
        }

        public override void Setup(Mock<T> mock, T proxy)
        {
            mock.Setup(this.MockExpresison).Returns(new InvocationFunc(invocation =>
            {
                Type[] genericArguments = invocation.Method.GetGenericArguments();
                MethodInfo methodInfo = this.GetMethodInfo(genericArguments);
                return methodInfo.Invoke(proxy, [.. invocation.Arguments])!;
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
    }
}
