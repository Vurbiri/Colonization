using UnityEngine;

public static class URandom
{
    public static bool IsTrue(int chanceTrue = 50) => chanceTrue > 0 && (chanceTrue >= 100 || Random.Range(0, 100) < chanceTrue);

    public static Vector2 Vector2(Vector2 minInclusive, Vector2 maxInclusive) => new(Random.Range(minInclusive.x, maxInclusive.x), Random.Range(minInclusive.y, maxInclusive.y));

    public static float RangeZero(float max) => Random.Range(0f, max);
}
