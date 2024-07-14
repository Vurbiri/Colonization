using UnityEngine;

public static class URandom
{
    public static Vector2 Vector2(Vector2 minInclusive, Vector2 maxInclusive) => new(Random.Range(minInclusive.x, maxInclusive.x), Random.Range(minInclusive.y, maxInclusive.y));
}
