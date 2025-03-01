using Moq;
using MoqProxy.Tests.Extensions;
using MoqProxy.Tests.Fixtures;
using System.Linq.Expressions;

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
        public void ActionWithArguments_IsProxied()
        {
            int anyInt = It.IsAny<int>();
            Expression expression = Expression.Constant(anyInt, typeof(int));

            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.ActionWithArguments(1337, "Hello World"),
                times: Times.Once);
        }

        [Fact]
        public void GenericActionWithArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.GenericActionWithArguments<int, string>(1337_420, "xyz"),
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

        [Fact]
        public void FuncWithArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeExpressionThenVerifyProxied(
                expression: x => x.FuncWithArguments(123_456, "abc"),
                times: Times.Once,
                expectedResult: 789_000);
        }
    }
}