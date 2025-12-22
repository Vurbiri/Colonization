using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Collections
{
    [Serializable, JsonArray]
    public class ReadOnlyArray<TValue> : IReadOnlyList<TValue>, ISerializationCallbackReceiver
    {
        protected static readonly IEqualityComparer<TValue> s_comparer = EqualityComparer<TValue>.Default;
        protected static readonly TValue[] s_empty = new TValue[0];

        [SerializeField] protected TValue[] _values;

        protected readonly Version _version = new();
        protected int _count;

        public TValue this[int index] { [Impl(256)] get => _values[index]; }

        public int Count { [Impl(256)] get => _count; }

        [Impl(256)] public ReadOnlyArray(int count)
        {
            _values = new TValue[count];
            _count = count;
        }
        [Impl(256)] public ReadOnlyArray(params TValue[] values)
        {
            _values = values;
            _count = values.Length;
        }
        [JsonConstructor]
        public ReadOnlyArray(ICollection<TValue> collection)
        {
            _count = collection.Count;
            _values = new TValue[_count];
            collection.CopyTo(_values, 0);
        }
        protected ReadOnlyArray() { }

        public int IndexOf(TValue item)
        {
            int i = _count;
            while (i --> 0 && !s_comparer.Equals(_values[i], item));
            return i;
        }
        [Impl(256)] public bool Contains(TValue item) => IndexOf(item) >= 0;

        [Impl(256)] public TValue Rand() => _values[UnityEngine.Random.Range(0, _count)];

        [Impl(256)] public TValue Prev(int index) => _values[(index == 0 ? _count : index) - 1];
        [Impl(256)] public TValue Next(int index) => _values[(index + 1) % _count];

        [Impl(256)] public int LeftIndex(int index) => (index == 0 ? _count : index) - 1;
        [Impl(256)] public int RightIndex(int index) => (index + 1) % _count;

        [Impl(256)] public void CopyTo(TValue[] array, int arrayIndex) => Array.Copy(_values, 0, array, arrayIndex, _count);

        [Impl(256)] public ArrayEnumerator<TValue> GetEnumerator() => new(_values, _count, _version);
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => _count == 0 ? EmptyEnumerator<TValue>.Instance : GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TValue>)this).GetEnumerator();

        
        [Impl(256)] public static implicit operator ReadOnlyArray<TValue>(TValue[] values) => new(values);

        public void OnAfterDeserialize() => _count = _values != null ? _values.Length : -1;
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            OnAfterDeserialize();
#endif
        }
    }

    //sealed internal class ArrayGenericConverter : JsonConverter
    //{
    //    private readonly Type _type = typeof(ReadOnlyArray<>);

    //    public override bool CanConvert(Type objectType) => objectType != null && (objectType.IsGenericType & objectType.GetGenericTypeDefinition().IsAssignableFrom(_type));

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        return Activator.CreateInstance(objectType, serializer.Deserialize(reader, objectType.GenericTypeArguments[0].MakeArrayType()));
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        var array = (IEnumerable)value;

    //        writer.WriteStartArray();
    //        {
    //            foreach (var item in array)
    //                writer.WriteValue(item);
    //        }
    //        writer.WriteEndArray();
    //    }
    //}
}
