using System.Linq.Expressions;
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
            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.ActionNoArguments(),
                times: Times.Once);
        }

        [Fact]
        public void GenericActionNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.GenericActionNoArguments<int>(),
                times: Times.Once);
        }

        [Fact]
        public void ActionWithArguments_IsProxied()
        {
            int anyInt = It.IsAny<int>();
            Expression expression = Expression.Constant(anyInt, typeof(int));

            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.ActionWithArguments(1337, "Hello World"),
                times: Times.Once);
        }

        [Fact]
        public void GenericActionWithArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.GenericActionWithArguments<int, string>(1337_420, "xyz"),
                times: Times.Once);
        }

        [Fact]
        public void ActionWithRefArgument_IsProxied()
        {
            int anyInt = It.IsAny<int>();
            Expression expression = Expression.Constant(anyInt, typeof(int));

            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.ActionWithRefArgument(ref It.Ref<int>.IsAny),
                times: Times.Once);
        }

        [Fact]
        public void ActionWithOutArgument_IsProxied()
        {
            int anyInt = It.IsAny<int>();
            Expression expression = Expression.Constant(anyInt, typeof(int));

            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.ActionWithOutArgument(out It.Ref<int>.IsAny),
                times: Times.Once);
        }

        [Fact]
        public void ActionWithInArgument_IsProxied()
        {
            int anyInt = It.IsAny<int>();
            Expression expression = Expression.Constant(anyInt, typeof(int));

            this.TestServiceMockProxy.InvokeActionThenVerifyProxied(
                expression: x => x.ActionWithInArgument(in It.Ref<int>.IsAny),
                times: Times.Once);
        }

        [Fact]
        public void FuncNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeFuncThenVerifyProxied(
                expression: x => x.FuncNoArguments(),
                times: Times.Once,
                expectedResult: 69_420);
        }

        [Fact]
        public void GenericFuncNoArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeFuncThenVerifyProxied(
                expression: x => x.GenericFuncNoArguments<int>(),
                times: Times.Once,
                expectedResult: 69_420);
        }

        [Fact]
        public void FuncWithArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeFuncThenVerifyProxied(
                expression: x => x.FuncWithArguments(123_456, "abc"),
                times: Times.Once,
                expectedResult: 789_000);
        }

        [Fact]
        public void GenericFuncWithArguments_IsProxied()
        {
            this.TestServiceMockProxy.InvokeFuncThenVerifyProxied(
                expression: x => x.GenericFuncWithArguments<int, string>(789_000, "xyz"),
                times: Times.Once,
                expectedResult: 101_101);
        }

        [Fact]
        public void OverwrittenProxyMethod_DoesNotInvokeTarget()
        {
            // Overwride proxy
            this.TestServiceMockProxy.MockProxy.Setup(x => x.ActionNoArguments()).Callback(() => { });

            // Invoke Proxy
            this.TestServiceMockProxy.MockProxy.Object.ActionNoArguments();

            // Verify target not invoked
            this.TestServiceMockProxy.TargetMock.Verify(x => x.ActionNoArguments(), Times.Never);
        }
    }
}