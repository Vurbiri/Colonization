//Assets\Vurbiri\Runtime\Types\Random\RColor32.cs
using System;
using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public class RColor32 : ISerializationCallbackReceiver
    {
        [SerializeField] private Color32 _color = new(0, 0, 0, 255);
        [SerializeField, Range(0f, 1f)] private float _range = 0.2f;
        [SerializeField] private bool _isAlphaRandom = false;

        public Color32 Color => _currentColor;
        public Color32 Roll
        {
            get
            {
                _currentColor = _color;
                for (int i = 0; i < _count; i++)
                    _currentColor[i] = (byte)Mathf.Clamp(_currentColor[i] * _rand, 0f, 255f);
                return _currentColor;
            }
        }

        private Color32 _currentColor;
        private int _count = 3;
        private RFloat _rand;

        public void Rolling()
        {
            _currentColor = _color;
            for (int i = 0; i < _count; i++)
                _currentColor[i] = (byte)Mathf.Clamp(_currentColor[i] * _rand, 0f, 255f);
        }

        public static implicit operator Color32(RColor32 value) => value._currentColor;

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() 
        {
            _currentColor = _color;
            _count = _isAlphaRandom ? 4 : 3;
            _rand = new(1f - _range, 1f);
        }

        public override string ToString() => _currentColor.ToString();
    }
}
