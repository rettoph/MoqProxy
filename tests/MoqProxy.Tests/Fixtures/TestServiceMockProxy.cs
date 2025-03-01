using Moq;
using MoqProxy.Tests.Stubs;

namespace MoqProxy.Tests.Fixtures
{
    public class TestServiceMockProxy
    {
        public Mock<ITestService> Mock { get; }
        public MockProxy<ITestService, ITestService> MockProxy { get; }

        public TestServiceMockProxy()
        {
            this.Mock = new Mock<ITestService>();
            this.MockProxy = new MockProxy<ITestService, ITestService>(this.Mock.Object);
        }
    }
}
