//Assets\Vurbiri.UI\Runtime\Abstract\AVProgressBar.cs
using System;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [ExecuteAlways]
    public abstract class AVProgressBar<T> : MonoBehaviour, ICanvasElement where T : struct, IEquatable<T>, IComparable<T>
    {
        private const int HORIZONTAL = 0, VERTICAL = 1;

        [SerializeField] private RectTransform _fillRect;
        [SerializeField] private Gradient _gradient = new();
        [SerializeField] private Direction _direction = Direction.LeftToRight;
        [SerializeField] protected T _minValue;
        [SerializeField] protected T _maxValue;
        [SerializeField] protected T _value;
        [SerializeField] private UniSigner<T> _onValueChanged = new();

        protected float _normalizedValue;
        private int _axis;
        private bool _reverseValue;

        private RectTransform _thisRectTransform;

        private Graphic _fillGraphic;
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
        public T Value
        {
            get => _value;
            set => Set(value, true);
        }
        public T SilentValue
        {
            get => _value;
            set => Set(value, false);
        }
        public T MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue.Equals(value) | _maxValue.CompareTo(value) <= 0) return;

                _minValue = value;
                UpdateMinMaxDependencies();
            }
        }
        public T MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue.Equals(value) | _minValue.CompareTo(value) >= 0) return;

                _maxValue = value;
                UpdateMinMaxDependencies();
            }
        }
        public RectTransform FillRect
        {
            get => _fillRect;
            set
            {
                if (_fillRect == value) return;

                _fillRect = value;
                UpdateFillRectReferences();
                UpdateVisuals();
            }
        }
        public Direction Direction
        {
            get => _direction;
            set
            {
                if (_direction == value) return;

                _direction = value;
                UpdateDirection(value);
                UpdateVisuals();
            }
        }
        #endregion


        public Unsubscriber AddListener(Action<T> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _value);
        public void RemoveListener(Action<T> action) => _onValueChanged.Remove(action);

        public bool SetMinMax(T min, T max)
        {
            if (min.CompareTo(max) >= 0) return false;
            if (min.Equals(_minValue) & max.Equals(_maxValue)) return true;

            _maxValue = min; _maxValue = max;
            UpdateMinMaxDependencies();
            return true;
        }

        protected void Set(T value, bool sendCallback)
        {
            value = ClampValue(value);

            if (_value.Equals(value)) return;

            _value = value;
            Normalized(value);

            UpdateVisuals();

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("VSlider.value", this);
                _onValueChanged.Invoke(value);
            }
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
            Normalized(_value);
            UpdateVisuals();
        }

        private void UpdateFillRectReferences()
        {
            if (_fillRect && _fillRect != _thisRectTransform)
            {
                _fillGraphic = _fillRect.GetComponent<Graphic>();
                _fillImage = _fillGraphic as Image;
                if (_fillRect.parent != null)
                    _fillContainerRect = _fillRect.parent.GetComponent<RectTransform>();
            }
            else
            {
                _fillRect = null;
                _fillGraphic = null;
                _fillImage = null;
                _fillContainerRect = null;
            }
        }

        protected void UpdateVisuals()
        {
#if UNITY_EDITOR
            Update_Editor();
#endif

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

                if (_fillGraphic != null)
                    _fillGraphic.canvasRenderer.SetColor(_gradient.Evaluate(_normalizedValue));

                _fillRect.anchorMin = anchorMin;
                _fillRect.anchorMax = anchorMax;
            }
        }

        private void UpdateDirection(Direction direction)
        {
            int oldAxis = _axis;
            bool oldReverse = _reverseValue;

            _axis = (direction == Direction.LeftToRight | direction == Direction.RightToLeft) ? HORIZONTAL : VERTICAL;
            _reverseValue = direction == Direction.RightToLeft | direction == Direction.TopToBottom;

            if (_axis != oldAxis) RectTransformUtility.FlipLayoutAxes(_thisRectTransform, true, true);
            if (_reverseValue != oldReverse) RectTransformUtility.FlipLayoutOnAxis(_thisRectTransform, _axis, true, true);
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
            UpdateDirection(_direction);
            Normalized(_value);
            UpdateVisuals();

            _onValueChanged.Init();
        }

        private void OnDisable()
        {
            _tracker.Clear();
        }

        private void OnRectTransformDimensionsChange()
        {
            Debug.Log("OnRectTransformDimensionsChange");
            if (isActiveAndEnabled)
                UpdateVisuals();
        }
        #endregion

        #region ICanvasElement
        public void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                _onValueChanged.Invoke(_value);
#endif
        }
        public bool IsDestroyed()
        {
            return this == null;
        }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion


#if UNITY_EDITOR
        private void Update_Editor()
        {
            if (!Application.isPlaying)
            {
                _axis = (_direction == Direction.LeftToRight | _direction == Direction.RightToLeft) ? HORIZONTAL : VERTICAL;
                _reverseValue = _direction == Direction.RightToLeft | _direction == Direction.TopToBottom;

                if (_thisRectTransform == null)
                    _thisRectTransform = (RectTransform)transform;
                UpdateFillRectReferences();
            }
        }
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                Update_Editor();

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
#endif
    }
}
