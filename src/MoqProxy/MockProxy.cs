using System.Reflection;
using Moq;
using MoqProxy.MemberProxies;

namespace MoqProxy
{
    public class MockProxy<TMock, TTarget> : Mock<TMock>
        where TMock : class
        where TTarget : class, TMock
    {
        public TTarget Target { get; }

        public MockProxy(TTarget target)
        {
            this.Target = target;
            List<IMemberProxy<TMock>> memberProxies = [];

            MethodInfo[] methodInfos = typeof(TMock).GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                IMemberProxy<TMock> methodProxy = methodInfo.ReturnType.Equals(typeof(void))
                    ? ActionProxy<TMock>.Get(methodInfo)
                    : FuncProxy<TMock>.Get(methodInfo);

                memberProxies.Add(methodProxy);
            }

            foreach (IMemberProxy<TMock> memberProxy in memberProxies)
            {
                memberProxy.Setup(this, this.Target);
            }
        }
    }

    public class MockProxy<T>(T target) : MockProxy<T, T>(target)
        where T : class
    {

    }
}
