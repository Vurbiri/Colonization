#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.Collections
{
    public partial class ReadOnlyIdSet<TId, TValue> : ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if (_values.Length != IdType<TId>.Count)
                Array.Resize(ref _values, IdType<TId>.Count);

            TValue value; _count = 0;
            for (int index, i = 0; i < IdType<TId>.Count; i++)
            {
                value = _values[i];
                if (value != null)
                {
                    for (int j = i + 1; j < IdType<TId>.Count; j++)
                        if (_values[j] != null && value.Id == _values[j].Id)
                            _values[j] = null;

                    if ((index = value.Id.Value) == i)
                    { _count++; }
                    else
                    { (_values[i], _values[index]) = (_values[index], _values[i]); i--; }
                }
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}

#endif
