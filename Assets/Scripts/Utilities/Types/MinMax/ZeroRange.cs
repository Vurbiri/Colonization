using UnityEngine;

[System.Serializable]
public class ZeroRange 
{
    [SerializeField] private float _value;

    public float Rand => Random.Range(0f, _value);

    public ZeroRange(float value) => _value = value;

    public static implicit operator ZeroRange(float value) => new(value);
}
