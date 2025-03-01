using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoqProxy.Extensions
{
    internal static class ParameterInfoExtensions
    {
        private static MethodInfo _itIsAnyMethodInfo = typeof(It).GetMethod(nameof(It.IsAny), 1, []) ?? throw new NotImplementedException();
        public static Expression GetExpression(this ParameterInfo parameterInfo)
        {
            MethodInfo methodInfo = _itIsAnyMethodInfo.MakeGenericMethod(parameterInfo.ParameterType);
            return Expression.Call(methodInfo);
        }
    }
}
