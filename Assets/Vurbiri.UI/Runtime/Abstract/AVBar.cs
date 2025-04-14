//Assets\Vurbiri.UI\Runtime\Abstract\AVBar.cs
using System;
using UnityEngine;

namespace Vurbiri.UI
{
    public abstract class AVBar<T> : AVBarBase where T : struct, IEquatable<T>, IComparable<T>
    {
        [SerializeField] protected T _value;
        [SerializeField] protected T _minValue;
        [SerializeField] protected T _maxValue;

        #region Abstract
        public abstract float NormalizedValue { get; set; }
        protected abstract void Normalized(T value);
        #endregion

        #region Properties
        public T Value
        {
            get => _value;
            set
            {
                value = ClampValue(value);
                
                if (!_value.Equals(value))
                {
                    _value = value;
                    Normalized(value);
                    
                    UpdateVisuals();
                }
            }
        }
        public T MinValue
        {
            get => _minValue;
            set
            {
                if (!_minValue.Equals(value) & _maxValue.CompareTo(value) > 0)
                {
                    _minValue = value;
                    UpdateMinMaxDependencies();
                }
            }
        }
        public T MaxValue
        {
            get => _maxValue;
            set
            {
                if (!_maxValue.Equals(value) & _minValue.CompareTo(value) < 0)
                {
                    _maxValue = value;
                    UpdateMinMaxDependencies();
                }
            }
        }
        #endregion

        public bool SetMinMax(T min, T max)
        {
            if (min.CompareTo(max) >= 0) 
                return false;
            if (!min.Equals(_minValue) | !max.Equals(_maxValue))
            {
                _minValue = min; _maxValue = max;
                UpdateMinMaxDependencies();
            }
            return true;
        }

        private T ClampValue(T value)
        {
            if (value.CompareTo(_minValue) < 0)
                value = _minValue;
            else if (value.CompareTo(_maxValue) > 0)
                value = _maxValue;

            return value;
        }

        private void UpdateMinMaxDependencies()
        {
            _value = ClampValue(_value);
            Normalized(_value);
            UpdateVisuals();
        }

        private void Start()
        {
            UpdateMinMaxDependencies();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!Application.isPlaying)
            {
                _value = ClampValue(_value);
                Normalized(_value);
            }
        }
#endif
    }
}
