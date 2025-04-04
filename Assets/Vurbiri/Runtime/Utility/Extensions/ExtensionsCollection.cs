//Assets\Vurbiri\Runtime\Utility\Extensions\ExtensionsCollection.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsCollection
    {
        public static int LeftIndex<T>(this IReadOnlyCollection<T> self, int index) => (index == 0 ? self.Count : index) - 1;
        public static int RightIndex<T>(this IReadOnlyCollection<T> self, int index) => (index + 1) % self.Count;

        public static T Prev<T>(this IReadOnlyList<T> self, int index) => self[(index == 0 ? self.Count : index) - 1];
        public static T Next<T>(this IReadOnlyList<T> self, int index) => self[(index + 1) % self.Count];

        public static T Rand<T>(this IReadOnlyList<T> self) => self[Random.Range(0, self.Count)];
        public static T Rand<T>(this IReadOnlyList<T> self, int startIndex) => self[Random.Range(startIndex, self.Count)];

        public static int FirstNullIndex<T>(this IReadOnlyList<T> self) where T : class
        {
            for (int i = 0; i < self.Count; i++)
                if (self[i] == null)
                    return i;
            return -1;
        }

        public static void FillDefault<T>(this IList<T> self, T value = default)
        {
            for (int i = 0; i < self.Count; i++)
                self[i] = value;
        }

        public static IList<int> Fill(this IList<int> self, int start = 0)
        {
            for (int i = 0, v = start; i < self.Count; i++, v++)
                self[i] = v;

            return self;
        }

        public static T[] ToArray<T>(this ICollection<T> self)
        {
            T[] array = new T[self.Count];
            self.CopyTo(array, 0);
            return array;
        }

        public static void Resize<T>(this List<T> self, int size) where T : new()
        {
            int count = self.Count;

            if (size == count)
                return;

            if (size > count)
            {
                if (size > self.Capacity)
                    self.Capacity = size;
                self.AddRange(Enumerable.Repeat<T>(new(), size - count));
                return;
            }

            if (size < count)
                self.RemoveRange(size, count - size);
        }

        public static T First<T>(this IReadOnlyCollection<T> self)
        {
            Throw.IfLengthZero<T>(self);

            IEnumerator<T> enumerator = self.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }

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
