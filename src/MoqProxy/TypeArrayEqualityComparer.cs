using System.Diagnostics.CodeAnalysis;

namespace MoqProxy
{
    internal class TypeArrayEqualityComparer : EqualityComparer<Type[]>
    {
        public static readonly TypeArrayEqualityComparer Instance = new();
        public override bool Equals(Type[]? x, Type[]? y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Equals(y[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode([DisallowNull] Type[] obj)
        {
            if (obj.Length == 0)
            {
                return 0;
            }

            int hashCode = HashCode.Combine(obj[0].GetHashCode());
            for (int i = 1; i < obj.Length; i++)
            {
                hashCode = HashCode.Combine(obj[i].GetHashCode());
            }
            return hashCode;
        }
    }
}
