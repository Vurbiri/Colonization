using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsCollection
{
    public static int LeftIndex<T>(this IReadOnlyList<T> self, int index) => (index == 0 ? self.Count : index) - 1;
    public static int RightIndex<T>(this IReadOnlyList<T> self, int index) => (index + 1) % self.Count;
   
    public static T Next<T>(this IReadOnlyList<T> self, int index) => self[(index + 1) % self.Count];

    public static void Shuffle<T>(this IList<T> self)
    {
        for (int i = self.Count - 1, j; i > 0; i--)
        {
            j = Random.Range(0, i + 1);
            (self[j], self[i]) = (self[i], self[j]);
        }
    }

    public static T GetValue<T>(this IEnumerable<T> self, int index)
    {
        IEnumerator<T> enumerator = self.GetEnumerator();
        enumerator.Reset();
        for (int i = 0; i <= index; i++)
            enumerator.MoveNext();
        
        return enumerator.Current;
    }

    public static bool BinaryContains<T>(this List<T> self, T item) => self.BinarySearch(item) >= 0;

    public static T Pop<T>(this IList<T> self)
    {
        T obj = self[^1]; self.RemoveAt(self.Count - 1);
        return obj;
    }

    public static T Rand<T>(this IReadOnlyList<T> self) => self[Random.Range(0, self.Count)];

    public static bool IsCorrect<T>(this T[,] self, Vector2Int index) => index.x >= 0 && index.x < self.GetLength(0) && index.y >= 0 && index.y < self.GetLength(1);
}
