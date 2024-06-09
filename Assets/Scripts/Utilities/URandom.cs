using UnityEngine;

public static class URandom
{
    public static bool IsTrue(int chanceTrue = 50) => chanceTrue > 0 && (chanceTrue >= 100 || Random.Range(0, 100) < chanceTrue);

    public static Vector2 Vector2(Vector2 minInclusive, Vector2 maxExclusive) => new(Random.Range(minInclusive.x, maxExclusive.x), Random.Range(minInclusive.y, maxExclusive.y));
    public static Vector2Int Vector2Int(Vector2Int minInclusive, Vector2Int maxExclusive) => new(Random.Range(minInclusive.x, maxExclusive.x), Random.Range(minInclusive.y, maxExclusive.y));


    public static float Range(Vector2 min_max) => Random.Range(min_max.x, min_max.y);
    public static int Range(Vector2Int min_max) => Random.Range(min_max.x, min_max.y);
}
