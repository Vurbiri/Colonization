using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri
{
    public static class CollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LeftIndex<T>(this IReadOnlyCollection<T> self, int index) => (index == 0 ? self.Count : index) - 1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RightIndex<T>(this IReadOnlyCollection<T> self, int index) => (index + 1) % self.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Prev<T>(this IReadOnlyList<T> self, int index) => self[(index == 0 ? self.Count : index) - 1];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Next<T>(this IReadOnlyList<T> self, int index) => self[(index + 1) % self.Count];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Rand<T>(this IReadOnlyList<T> self) => self[Random.Range(0, self.Count)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandE<T>(this ICollection<T> self)
        {
            int index = Random.Range(0, self.Count);
            using IEnumerator<T> enumerator = self.GetEnumerator();
            while (enumerator.MoveNext() & index --> 0) { }
            return enumerator.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FirstNullIndex<T>(this IReadOnlyList<T> self) where T : class
        {
            for (int i = 0; i < self.Count; i++)
                if (self[i] == null)
                    return i;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArray<T>(this ICollection<T> self)
        {
            T[] array = new T[self.Count];
            self.CopyTo(array, 0);
            return array;
        }

        public static void Resize<T>(this List<T> self, int size) where T : new()
        {
            int count = self.Count;

            if (size > count)
            {
                if (size > self.Capacity)  self.Capacity = size;
                while (self.Count != size) self.Add(new());
            }
            else if (size < count)
            {
                self.RemoveRange(size, count - size);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Any<T>(this IEnumerable<T> self)
        {
            using IEnumerator<T> enumerator = self.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this IList<T> self)
        {
            for (int i = self.Count - 1, j; i > 0; i--)
            {
                j = Random.Range(0, i);
                (self[j], self[i]) = (self[i], self[j]);
            }
        }
    }
}
