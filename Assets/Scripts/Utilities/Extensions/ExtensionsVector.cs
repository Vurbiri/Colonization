using UnityEngine;

public static class ExtensionsVector
{
    public static Vector2 Offset(this Vector2 self, float value) => self + value * Vector2.one;
    public static Vector3[] OffsetSelf(this Vector3[] self, Vector3 value)
    {
        for (int i = 0; i < self.Length; i++)
            self[i] += value;

        return self;
    }

    public static Vector2 To2D(this Vector3 self) => new(self.x, self.z);
    public static Vector3 To3D(this Vector2 self, float z = 0) => new(self.x, z, self.y);

    public static float[] ToArray(this Vector3 self) => new[] { self.x, self.y, self.z };
    public static float[] ToArray(this Vector2 self) => new[] { self.x, self.y };


    public static bool IsBetween(this Vector2Int self, Vector2Int a, Vector2Int b) =>
        ((self.x == a.x && self.x == b.x) && ((a.y < b.y && a.y <= self.y && self.y <= b.y) || (a.y > b.y && b.y <= self.y && self.y <= a.y))) ||
        ((self.y == a.y && self.y == b.y) && ((a.x < b.x && a.x <= self.x && self.x <= b.x) || (a.x > b.x && b.x <= self.x && self.x <= a.x)));


    public static Vector2Int NormalizeDirection(this Vector2Int self)
    {
        int length = Mathf.Abs(self.x + self.y);

        return new(self.x /= length, self.y /= length);
    }

    public static void Turn90Right(this ref Vector2Int self)
    {
        int x = self.x;

        self.x = self.y;
        self.y = -x;
    }
    public static void Turn90Left(this ref Vector2Int self)
    {
        int x = self.x;

        self.x = -self.y;
        self.y = x;
    }

    public static Vector3 ToVector3(this Vector2Int self) => new(self.x, self.y, 0f);
}
