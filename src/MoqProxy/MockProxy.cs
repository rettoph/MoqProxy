using Moq;
using MoqProxy.MemberProxies;
using System.Reflection;

namespace MoqProxy
{
    public class MockProxy<TMock, TProxy> : Mock<TMock>
        where TMock : class
        where TProxy : class, TMock
    {
        private readonly TProxy _proxied;
        public TProxy Proxied => this._proxied;

        public MockProxy(TProxy proxied)
        {
            this._proxied = proxied;
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
                memberProxy.Setup(this, this._proxied);
            }
        }
    }
}
