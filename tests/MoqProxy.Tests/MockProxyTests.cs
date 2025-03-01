using Moq;
using MoqProxy.Tests.Extensions;
using MoqProxy.Tests.Fixtures;

namespace MoqProxy.Tests
{
    public class MockProxyTests
    {
        public TestServiceMockProxy TestServiceMockProxy;

        public MockProxyTests()
        {
            this.TestServiceMockProxy = new TestServiceMockProxy();
        }

        [Fact]
        public void ActionNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.ActionNoArguments(),
                times: Times.Once);
        }

        [Fact]
        public void GenericActionNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.GenericActionNoArguments<int>(),
                times: Times.Once);
        }

        [Fact]
        public void FuncNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.FuncNoArguments(),
                times: Times.Once,
                expectedResult: 69_420);
        }

        [Fact]
        public void GenericFuncNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.GenericFuncNoArguments<int>(),
                times: Times.Once,
                expectedResult: 69_420);
        }
    }
}