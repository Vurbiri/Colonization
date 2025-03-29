//Assets\Vurbiri\Runtime\Types\Collections\EnumCollections\Editor\EnumArray_Editor.cs
#if UNITY_EDITOR

using System;
using UnityEngine;

namespace Vurbiri.Collections
{
    public partial class EnumArray<TType, TValue> : ISerializationCallbackReceiver
    {
        public virtual void OnBeforeSerialize()
        {
            if (Application.isPlaying)
                return;

            _count = Enum<TType>.Count;
            if (_values.Length != _count)
                Array.Resize(ref _values, _count);
        }
        public void OnAfterDeserialize() { }
    }
}

#endif
