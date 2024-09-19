using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vurbiri
{
    public static class ExtensionsCollection
    {
        public static int LeftIndex<T>(this IReadOnlyList<T> self, int index) => (index == 0 ? self.Count : index) - 1;
        public static int RightIndex<T>(this IReadOnlyList<T> self, int index) => (index + 1) % self.Count;

        public static T Prev<T>(this IReadOnlyList<T> self, int index) => self[(index == 0 ? self.Count : index) - 1];
        public static T Next<T>(this IReadOnlyList<T> self, int index) => self[(index + 1) % self.Count];

        public static T Rand<T>(this IReadOnlyList<T> self) => self[Random.Range(0, self.Count)];
        public static T Rand<T>(this IReadOnlyList<T> self, int startIndex) => self[Random.Range(startIndex, self.Count)];

        public static void Fill<T>(this IList<T> self, T value = default)
        {
            for (int i = 0; i < self.Count; i++)
                self[i] = value;
        }

        public static T[] ToArray<T>(this ICollection<T> self)
        {
            T[] arr = new T[self.Count];
            self.CopyTo(arr, 0);
            return arr;
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

        public static T First<T>(this ICollection<T> self)
        {
            if (self.Count == 0)
                throw new System.IndexOutOfRangeException();

            IEnumerator<T> enumerator = self.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }

        public static T GetValue<T>(this ICollection<T> self, int index)
        {
            if (index < 0 || index >= self.Count)
                throw new System.IndexOutOfRangeException();

            IEnumerator<T> enumerator = self.GetEnumerator();
            enumerator.Reset();
            for (int i = 0; i <= index; i++)
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

        public static T Pop<T>(this IList<T> self)
        {
            T obj = self[^1]; self.RemoveAt(self.Count - 1);
            return obj;
        }

        public static bool IsCorrectIndex<T>(this T[,] self, Vector2Int index) => index.x >= 0 && index.x < self.GetLength(0) && index.y >= 0 && index.y < self.GetLength(1);

        public static bool TryAdd<T>(this ISet<T> self, T value)
        {
            if(self.Contains(value))
                return false;

            self.Add(value);
            return true;
        }
    }
}
