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

            if (_values.Length != count)
                Array.Resize(ref _values, count);
        }
        public void OnAfterDeserialize() { }
    }
}

#endif
