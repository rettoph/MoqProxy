using Moq;
using System.Linq.Expressions;
using System.Reflection;

namespace MoqProxy.Extensions
{
    internal static class MethodInfoExtensions
    {
        public static MethodInfo GetMockMethodInfo(this MethodInfo methodInfo)
        {
            if (methodInfo.IsGenericMethodDefinition == false)
            {
                return methodInfo;
            }

            Type[] mockGenericTypes = methodInfo.GetGenericArguments().Select(x => typeof(It.IsAnyType)).ToArray();
            return methodInfo.MakeGenericMethod(mockGenericTypes);
        }

        public static Expression<Action<T>> GetMockActionExpression<T>(this MethodInfo methodInfo)
        {
            MethodInfo mockMethodInfo = methodInfo.GetMockMethodInfo();
            ParameterExpression instance = Expression.Parameter(typeof(T), nameof(instance));
            MethodCallExpression methodCall = Expression.Call(instance, mockMethodInfo);

            return Expression.Lambda<Action<T>>(methodCall, instance);
        }

        public static Expression<Func<T, TOut>> GetMockFuncExpression<T, TOut>(this MethodInfo methodInfo)
        {
            MethodInfo mockMethodInfo = methodInfo.GetMockMethodInfo();
            ParameterExpression instance = Expression.Parameter(typeof(T), nameof(instance));
            MethodCallExpression methodCall = Expression.Call(instance, mockMethodInfo);

            return Expression.Lambda<Func<T, TOut>>(methodCall, instance);
        }
    }
}
