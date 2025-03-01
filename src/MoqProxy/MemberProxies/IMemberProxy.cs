using Moq;

namespace MoqProxy.MemberProxies
{
    public interface IMemberProxy<T>
        where T : class
    {
        void Setup(Mock<T> mock, T proxy);
    }
}
