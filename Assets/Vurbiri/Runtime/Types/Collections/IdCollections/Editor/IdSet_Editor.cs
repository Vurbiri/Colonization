//Assets\Vurbiri\Runtime\Types\Collections\IdCollections\Editor\IdSet_Editor.cs
#if UNITY_EDITOR

using System;
using UnityEngine;

namespace Vurbiri.Collections
{
    public partial class IdSet<TId, TValue> : ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            if (_values.Length != _capacity)
                Array.Resize(ref _values, _capacity);

            TValue value; _count = 0;
            for (int index, i = 0; i < _capacity; i++)
            {
                value = _values[i];
                if (value == null)
                    continue;

                for (int j = i + 1; j < _capacity; j++)
                {
                    if (_values[j] == null)
                        continue;

                    if (value.Id == _values[j].Id)
                        _values[j] = null;
                }

                index = value.Id.Value;
                if (index == i)
                {
                    _count++;
                    continue;
                }

                (_values[i], _values[index]) = (_values[index], _values[i]);
                i--;
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}

#endif
