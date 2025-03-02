using Moq;
using MoqProxy.Tests.Stubs;

namespace MoqProxy.Tests.Fixtures
{
    public class TestServiceMockProxy
    {
        public Mock<ITestService> TargetMock { get; }
        public MockProxy<ITestService, ITestService> MockProxy { get; }

        public TestServiceMockProxy()
        {
            this.TargetMock = new Mock<ITestService>();
            this.MockProxy = new MockProxy<ITestService, ITestService>(this.TargetMock.Object);
        }
    }
}
