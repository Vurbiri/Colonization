using UnityEngine;

[System.Serializable]
public struct MinusPlusRange
{
    [SerializeField] private float _value;

    public float Rand => Random.Range(-_value, _value);

    public MinusPlusRange(float value) => _value = value;

    public static implicit operator MinusPlusRange(float value) => new(value);
}
