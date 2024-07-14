using UnityEngine;

[System.Serializable]
public struct MinusPlus
{
    [SerializeField] private float _value;

    public readonly float Roll => Random.Range(-_value, _value);

    public MinusPlus(float value) => _value = value;

    public static implicit operator MinusPlus(float value) => new(value);

    public override string ToString() => $"(min: {-_value}, max: {_value})";
}
