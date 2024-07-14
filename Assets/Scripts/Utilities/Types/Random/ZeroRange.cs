using UnityEngine;

[System.Serializable]
public struct ZeroRange 
{
    [SerializeField] private float _value;

    public readonly float Roll => Random.Range(0f, _value);

    public ZeroRange(float value) => _value = value;

    public static float Rolling(float value) => Random.Range(0f, value);
    public static int Rolling(int value) => Random.Range(0, value);

    public static implicit operator ZeroRange(float value) => new(value);

    public override string ToString() => $"(min: {0}, max: {_value})";
}
