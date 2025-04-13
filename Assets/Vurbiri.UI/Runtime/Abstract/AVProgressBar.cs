//Assets\Vurbiri.UI\Runtime\Abstract\AVProgressBar.cs
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [ExecuteAlways]
    public abstract class AVProgressBar<T> : MonoBehaviour
#if UNITY_EDITOR
        , ICanvasElement
#endif
        where T : struct, IEquatable<T>, IComparable<T>
    {
        private const int HORIZONTAL = 0, VERTICAL = 1;

        [SerializeField] private RectTransform _fillRect;
        [SerializeField] private Direction _direction = Direction.LeftToRight;
        [SerializeField] protected T _minValue;
        [SerializeField] protected T _maxValue;
        [SerializeField] protected T _value;
        [SerializeField] private bool _useGradient = true;
        [SerializeField] private Gradient _gradient = new();

        protected float _normalizedValue;
        private int _axis;
        private bool _reverseValue;

        private RectTransform _thisRectTransform;

        private CanvasRenderer _fillRenderer;
        private Image _fillImage;
        private RectTransform _fillContainerRect;

#pragma warning disable 649
        private DrivenRectTransformTracker _tracker;  // field is never assigned warning
#pragma warning restore 649

        #region Abstract
        public abstract float NormalizedValue { get; set; }
        protected abstract void Normalized(T value);
        #endregion

        #region Properties
        public RectTransform FillRect
        {
            get => _fillRect;
            set
            {
                if (_fillRect != value)
                {
                    _fillRect = value;
                    UpdateFillRectReferences();
                    UpdateVisuals();
                }
            }
        }
        public Direction Direction
        {
            get => _direction;
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    UpdateDirection(value, true);
                    UpdateVisuals();
                }
            }
        }
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
        public bool UseGradient
        {
            get => _useGradient;
            set
            {
                if (_useGradient != value)
                {
                    _useGradient = value;
                    UpdateColor();
                }
            }
        }
        public Gradient Gradient
        {
            get => _gradient;
            set 
            {
                if (_gradient != value)
                {
                    _gradient = value;
                    UpdateColor();
                }
            }
        }
        #endregion

        public bool SetMinMax(T min, T max)
        {
            if (min.CompareTo(max) >= 0) return false;
            if (min.Equals(_minValue) & max.Equals(_maxValue)) return true;

            _minValue = min; _maxValue = max;
            UpdateMinMaxDependencies();
            return true;
        }

        public void UpdateColor()
        {
            if (_fillRenderer != null)
                _fillRenderer.SetColor(_useGradient & _gradient != null ? _gradient.Evaluate(_normalizedValue) : Color.white);
        }

        private T ClampValue(T value)
        {
            if (value.CompareTo(_minValue) < 0)
                value = _minValue;
            else if (value.CompareTo(_maxValue) > 0)
                value = _maxValue;

            return value;
        }

        #region Update...
        private void UpdateMinMaxDependencies()
        {
            _value = ClampValue(_value);
            Normalized(_value);
            UpdateVisuals();
        }

        private void UpdateFillRectReferences()
        {
            _fillContainerRect = null;
            _fillRenderer = null;
            _fillImage = null;

            if (_fillRect != null & _fillRect != _thisRectTransform && _fillRect.parent != null)
            {
                _fillContainerRect = (RectTransform)_fillRect.parent;

                if (_fillRect.TryGetComponent<Graphic>(out var graphic))
                {
                    _fillRenderer = graphic.canvasRenderer;
                    _fillImage = graphic as Image;
                }
            }
            else
            {
                _fillRect = null;
            }
        }

        protected void UpdateVisuals()
        {
            _tracker.Clear();

            if (_fillContainerRect != null)
            {
                _tracker.Add(this, _fillRect, DrivenTransformProperties.Anchors);
                Vector2 anchorMin = Vector2.zero;
                Vector2 anchorMax = Vector2.one;

                if (_fillImage != null && _fillImage.type == Image.Type.Filled)
                {
                    _fillImage.fillAmount = _normalizedValue;
                }
                else
                {
                    if (_reverseValue)
                        anchorMin[_axis] = 1f - _normalizedValue;
                    else
                        anchorMax[_axis] = _normalizedValue;
                }

                if (_useGradient && _fillRenderer != null & _gradient != null)
                    _fillRenderer.SetColor(_gradient.Evaluate(_normalizedValue));

                _fillRect.anchorMin = anchorMin;
                _fillRect.anchorMax = anchorMax;
            }
        }

        private void UpdateDirection(Direction direction, bool flipLayout)
        {
            int oldAxis = _axis;
            bool oldReverse = _reverseValue;

            _axis = (direction == Direction.LeftToRight | direction == Direction.RightToLeft) ? HORIZONTAL : VERTICAL;
            _reverseValue = direction == Direction.RightToLeft | direction == Direction.TopToBottom;

            if (flipLayout & _axis != oldAxis) RectTransformUtility.FlipLayoutAxes(_thisRectTransform, true, true);
            if (flipLayout & _reverseValue != oldReverse) RectTransformUtility.FlipLayoutOnAxis(_thisRectTransform, _axis, true, true);
        }
        #endregion

        #region Calls ..
        private void Awake()
        {
            _thisRectTransform = (RectTransform)transform;
            UpdateFillRectReferences();
        }

        private void Start()
        {
            UpdateDirection(_direction, false);
            Normalized(_value);
            UpdateVisuals();
        }

        private void OnDisable()
        {
            _tracker.Clear();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (isActiveAndEnabled)
                UpdateVisuals();
        }
        #endregion

#if UNITY_EDITOR
        #region ICanvasElement
        public void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout)
            {
                UpdateMinMaxDependencies();
            }
        }
        public bool IsDestroyed() => this == null;
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                _thisRectTransform = (RectTransform)transform;
                UpdateFillRectReferences();

                UpdateDirection(_direction, false);

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
#endif
    }
}
