//Assets\Vurbiri.UI\Runtime\UIElements\VBarArithmetic.cs
using System;
using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.BAR_ARITHMETIC, VUI_CONST_ED.BAR_ORDER)]
    [RequireComponent(typeof(RectTransform))]
#endif
    sealed public class VBarArithmetic : AVBarBase
    {
        [SerializeField] private int _value;
        [SerializeField] private int _difference = 2;
        [SerializeField] private int _maxSteps = 10;

        private float _normalizedPart;
        private int _step;
        private int _maxValue;

        #region Properties
        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    ClampAndNormalized();
                    UpdateVisuals();
                }
            }
        }
        public int Difference
        {
            get => _difference;
            set
            {
                if (_difference != value & _difference > 0)
                {
                    _difference = value;
                    UpdateProgression();
                    UpdateVisuals();
                }
            }
        }
        public int MaxSteps
        {
            get => _maxSteps;
            set
            {
                if (_maxSteps != value & _maxSteps > 0)
                {
                    _maxSteps = value;
                    UpdateProgression();
                    UpdateVisuals();
                }
            }
        }
        public int MaxValue => _maxValue;
        public int Step => _step;
        public float NormalizedValue => _normalizedValue;
        #endregion

        public bool SetProgression(int difference, int maxSteps)
        {
            if (difference < 1 | maxSteps < 1) 
                return false;
            
            if (difference != _difference | maxSteps != _maxSteps)
            {
                _difference = difference; _maxSteps = maxSteps;
                UpdateProgression();
                UpdateVisuals();
            }
            return true;
        }

        private void ClampAndNormalized()
        {
            if (_value <= 0)
            {
                _normalizedValue = 0f;
                _value = 0;
                _step = 0;
                return;
            }

            if (_value >= _maxValue)
            {
                _normalizedValue = 1f;
                _value = _maxValue;
                _step = _maxSteps;
                return;
            }

            _step = ((int)Math.Sqrt(1.0 + (_value << 3) / (double)_difference) - 1) >> 1;
            _normalizedValue = _normalizedPart * (_value / (_difference * (_step + 1f)) + _step * 0.5f);
        }

        private void UpdateProgression()
        {
            _maxValue = _difference * _maxSteps * (_maxSteps + 1) >> 1;
            _normalizedPart = 1f / _maxSteps;

            ClampAndNormalized();
        }

        private void Start()
        {
            UpdateProgression();
            UpdateVisuals();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!Application.isPlaying)
                UpdateProgression();
        }
#endif
    }
}
