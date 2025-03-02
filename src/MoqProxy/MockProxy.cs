using Moq;
using MoqProxy.MemberProxies;
using System.Reflection;

namespace MoqProxy
{
    public class MockProxy<TMock, TTarget> : Mock<TMock>
        where TMock : class
        where TTarget : class, TMock
    {
        private readonly TTarget _target;
        public TTarget Target => this._target;

        public MockProxy(TTarget target)
        {
            this._target = target;
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
                memberProxy.Setup(this, this._target);
            }
        }
    }
}
