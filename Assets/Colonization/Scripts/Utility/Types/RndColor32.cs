using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable]
    public struct RndColor32
    {
        [SerializeField] private Color _color;
        [SerializeField, MinMax(-1f, 1f)] private RndFloat _range;
        [SerializeField] private bool _isAlpha;

        [NonSerialized] private Color32 _current;

        public readonly Color32 Current { [Impl(256)] get => _current; }
        public Color32 Roll { [Impl(256)] get { Next(); return _current; } }

        public void Next()
        {
            var current = _color;
            int count = _isAlpha ? 4 : 3;

            for (int i = 0; i < count; i++)
                current[i] *= 1f + _range;

            _current = current.ToColor32();
        }

        [Impl(256)] public static implicit operator Color32(RndColor32 value) => value._current;
    }
}
