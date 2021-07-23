using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Benchmarking.CSharp
{
    public class CollectionConcat<T> : IReadOnlyCollection<T>
    {
        private readonly IEnumerable<T> enumerableA;
        private readonly IEnumerable<T> enumerableB;

        public CollectionConcat(IEnumerable<T> a, IEnumerable<T> b)
        {
            enumerableA = a; enumerableB = b;
        }

        public int Count => enumerableA.Count() + enumerableB.Count();

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in enumerableA) { yield return item; }
            foreach (T item in enumerableB) { yield return item; }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
