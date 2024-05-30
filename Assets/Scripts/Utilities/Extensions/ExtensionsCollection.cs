using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsCollection
{
    public static int LeftIndex<T>(this T[] self, int index) => (index == 0 ? self.Length : index) - 1;
    public static int RightIndex<T>(this T[] self, int index) => (index + 1) % self.Length;
    public static int NextIndex<T>(this T[] self, int index) => (index + 1) % self.Length;
    public static T Next<T>(this T[] self, int index) => self[(index + 1) % self.Length];

    public static Vector2 ToVector2(this float[] self)
    {
        Vector2 vector = Vector2.zero;
        for (int i = 0; i < self.Length; i++)
        {
            if (i == 2)
                break;

            vector[i] = self[i];
        }
        return vector;
    }
    public static Vector3 ToVector3(this float[] self)
    {
        Vector3 vector = Vector3.zero;
        for (int i = 0; i < self.Length; i++)
        {
            if (i == 3)
                break;

            vector[i] = self[i];
        }
        return vector;
    }

    public static bool BinaryContains<T>(this List<T> self, T item) => self.BinarySearch(item) >= 0;

    public static T Pop<T>(this List<T> self)
    {
        T obj = self[^1]; self.RemoveAt(self.Count - 1);
        return obj;
    }

    public static T Rand<T>(this T[] self) => self[Random.Range(0, self.Length)];
    public static T Rand<T>(this List<T> self) => self[Random.Range(0, self.Count)];

    public static bool IsCorrect<T>(this T[,] self, Vector2Int index) => index.x >= 0 && index.x < self.GetLength(0) && index.y >= 0 && index.y < self.GetLength(1);
}
