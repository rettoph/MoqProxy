using Moq;
using MoqProxy.Tests.Fixtures;
using MoqProxy.Tests.Stubs;
using System.Linq.Expressions;

namespace MoqProxy.Tests.Extensions
{
    public static class TestServiceMockProxyExtensions
    {
        public static void InvokeActionThenVerifyProxied(
            this TestServiceMockProxy testServiceMockProxy,
            Expression<Action<ITestService>> expression,
            Func<Times> times)
        {
            // Invoke
            expression.Compile()(testServiceMockProxy.MockProxy.Object);

            // Verify
            testServiceMockProxy.MockProxy.Verify(expression, times);
            testServiceMockProxy.TargetMock.Verify(expression, times);
        }

        public static void InvokeFuncThenVerifyProxied<TOut>(
            this TestServiceMockProxy testServiceMockProxy,
            Expression<Func<ITestService, TOut>> expression,
            Func<Times> times,
            TOut expectedResult)
        {
            // Setup
            testServiceMockProxy.TargetMock.Setup(expression).Returns(expectedResult);

            // Invoke
            TOut result = expression.Compile()(testServiceMockProxy.MockProxy.Object);
            Assert.Equal(expectedResult, result);

            // Verify
            testServiceMockProxy.MockProxy.Verify(expression, times);
            testServiceMockProxy.TargetMock.Verify(expression, times);
        }
    }
}
