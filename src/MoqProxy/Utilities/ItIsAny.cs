using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MoqProxy.Utilities
{
    internal abstract class ItIsAny
    {
        public abstract Expression Constant { get; }

        private static readonly Dictionary<Type, Expression> _cache = [];
        public static Expression GetConstant(Type type)
        {
            ref Expression? expression = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, type, out bool exists);
            if(exists == true)
            {
                return expression!;
            }

            Type itIsAnyType = typeof(ItIsAny<>).MakeGenericType(type);
            ItIsAny itIsAny = (ItIsAny)(Activator.CreateInstance(itIsAnyType) ?? throw new NotImplementedException());
            expression =  itIsAny.Constant;

            return expression;
        }
    }

    internal class ItIsAny<T> : ItIsAny
    {
        public override Expression Constant => Expression.Constant(It.IsAny<T>(), typeof(T));
    }
}
