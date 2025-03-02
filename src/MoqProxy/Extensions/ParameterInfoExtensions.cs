using System.Linq.Expressions;
using System.Reflection;
using Moq;

namespace MoqProxy.Extensions
{
    internal static class ParameterInfoExtensions
    {
        private static readonly MethodInfo _itIsAnyMethodInfo = typeof(It).GetMethod(nameof(It.IsAny), 1, []) ?? throw new NotImplementedException();
        public static Expression GetExpression(this ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType.IsByRef)
            {
                Type nonRefType = parameterInfo.ParameterType.GetElementType() ?? throw new NotImplementedException();
                FieldInfo itRefIsAnyFieldInfo = typeof(It.Ref<>).MakeGenericType(nonRefType).GetField(nameof(It.Ref<object>.IsAny)) ?? throw new NotImplementedException();

                return Expression.Field(null, itRefIsAnyFieldInfo);
            }

            MethodInfo methodInfo = _itIsAnyMethodInfo.MakeGenericMethod(parameterInfo.ParameterType);
            return Expression.Call(methodInfo);
        }
    }
}
