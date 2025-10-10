using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class CollectionExtensions
    {
        [Impl(256)] public static int LeftIndex<T>(this IReadOnlyCollection<T> self, int index) => (index == 0 ? self.Count : index) - 1;
        [Impl(256)] public static int RightIndex<T>(this IReadOnlyCollection<T> self, int index) => (index + 1) % self.Count;

        [Impl(256)] public static T Prev<T>(this IReadOnlyList<T> self, int index) => self[(index == 0 ? self.Count : index) - 1];
        [Impl(256)] public static T Next<T>(this IReadOnlyList<T> self, int index) => self[(index + 1) % self.Count];

        [Impl(256)] public static T Rand<T>(this IReadOnlyList<T> self) => self[UnityEngine.Random.Range(0, self.Count)];

        [Impl(256)] public static T[] ToArray<T>(this ICollection<T> self)
        {
            T[] array = new T[self.Count];
            self.CopyTo(array, 0);
            return array;
        }

        [Impl(256)] public static T Any<T>(this IEnumerable<T> self)
        {
            using IEnumerator<T> enumerator = self.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }

        [Impl(256)] public static void Shuffle<T>(this IList<T> self)
        {
            for (int i = self.Count - 1, j; i > 0; i--)
            {
                j = UnityEngine.Random.Range(0, i);
                (self[j], self[i]) = (self[i], self[j]);
            }
        }
    }
}
