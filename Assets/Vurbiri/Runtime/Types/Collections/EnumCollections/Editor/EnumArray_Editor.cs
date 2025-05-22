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

            if (_values.Length != s_count)
                Array.Resize(ref _values, s_count);
        }
        public void OnAfterDeserialize() { }
    }
}

#endif
