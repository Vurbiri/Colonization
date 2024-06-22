using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnityDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] List<KeyValue> _elements;
    private Dictionary<TKey, TValue> _dictionary;

    public TValue this[TKey key] { get => _dictionary[key]; set => _dictionary[key] = value; }
    
    public Dictionary<TKey, TValue> Dictionary => _dictionary;

    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;
    
    public int Count => _dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value) => _dictionary.Add(key, value);
    public void Add(KeyValuePair<TKey, TValue> item) => _dictionary.Add(item.Key, item.Value);
    public bool TryAdd(TKey key, TValue value) => _dictionary.TryAdd(key, value);
    
    public void Clear() => _dictionary.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.ContainsKey(item.Key) && _dictionary.ContainsValue(item.Value);
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
    public bool ContainsValue(TValue value) => _dictionary.ContainsValue(value);

    public bool Remove(TKey key) => _dictionary.Remove(key);
    public bool Remove(TKey key, out TValue value) => _dictionary.Remove(key, out value);
    public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item.Key);

    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

    public void TrimExcess() => _dictionary.TrimExcess();
    public void TrimExcess(int capacity) => _dictionary.TrimExcess(capacity);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public void OnAfterDeserialize()
    {
        if (_elements == null)
            return;

        _dictionary = new(_elements.Count);

        foreach (var kv in _elements)
            _dictionary.TryAdd(kv.key, kv.value);
    }

    public void OnBeforeSerialize() { }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        int i = 0;
        foreach (var pair in _dictionary)
        {
            if (i == arrayIndex)
                break;

            array[i] = pair;
            ++i;
        }
    }

    public void DeleteList()
    {
        _elements?.Clear();
        _elements = null;
    }

    #region Nested: KeyValue
    //***********************************
    [Serializable]
    private struct KeyValue
    {
        public TKey key;
        public TValue value;

        public KeyValue(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
        private KeyValue(KeyValuePair<TKey, TValue> pair)
        {
            key = pair.Key;
            value = pair.Value;
        }

        public readonly override string ToString() => $"[{key}, {value}]";

        public static implicit operator KeyValue(KeyValuePair<TKey, TValue> pair) => new(pair);
        //public static implicit operator KeyValuePair<TKey, TValue>(KeyValue kv) => new(kv.key, kv.value);
    }
    #endregion

    #region Old OnAfterDeserialize & OnBeforeSerialize
    //public void OnAfterDeserialize()
    //{
    //    if (_elements == null)
    //        return;

    //    _dictionary = new(_elements.Count);

    //    TKey key;
    //    foreach (var kv in _elements)
    //    {
    //        key = kv.key;
    //        if (_dictionary.ContainsKey(key))
    //            key = default;

    //        if (!_dictionary.TryAdd(key, kv.value))
    //            Debug.LogError($"Не удалось добавить {kv} - элемент с таким же ключом [{key}] уже был добавлен.");
    //    }

    //    _elements.Clear();
    //    _elements = null;
    //}

    //public void OnBeforeSerialize()
    //{
    //if (_dictionary == null)
    //    return;

    //_elements = new(_dictionary.Count);

    //foreach (var kv in _dictionary)
    //    _elements.Add(kv);
    //}
    #endregion
}
