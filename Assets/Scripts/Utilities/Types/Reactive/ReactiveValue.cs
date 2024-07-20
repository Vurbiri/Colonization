using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class ReactiveValue<T>
{
    [SerializeField, JsonProperty("value")]
    private T _value;

    public T Value { get => _value; set { _value = value; EventChangeValue?.Invoke(value); } }

    private event Action<T> EventChangeValue;

    public ReactiveValue() => _value = default;
    public ReactiveValue(T value) => _value = value;


    public void Subscribe(Action<T> action)
    {
        EventChangeValue += action;
        action(_value);
    }
    public void Subscribe(Action<T> action, bool calling)
    {
        EventChangeValue += action;
        if (calling)
            action(_value);
    }

    public void UnSubscribe(Action<T> action) => EventChangeValue -= action;

    public static implicit operator ReactiveValue<T>(T value) => new(value);
}
