using System.Linq;
using System.Collections.Generic;

namespace Benchmarking.CSharp
{
    public static class Concat
    {
        public static IEnumerable<T> SelectManyConcat<T>(this IEnumerable<IEnumerable<T>> enumerable) =>
            enumerable.SelectMany(x => x);
        
        public static IEnumerable<T> CollectionConcat<T>(this IEnumerable<T> self, IEnumerable<T> other) =>
            new CollectionConcat<T>(self, other);

        public static IEnumerable<T> LinqConcat<T>(this IEnumerable<T> self, IEnumerable<T> other) =>
            self.Concat(other);
    }
}
