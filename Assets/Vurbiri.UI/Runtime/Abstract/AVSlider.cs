//Assets\Vurbiri.UI\Runtime\Abstract\AVSlider.cs
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public abstract class AVSlider<T> : VSelectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
		where T : struct, IEquatable<T>, IComparable<T>
	{
        private const int HORIZONTAL = 0, VERTICAL = 1;

        [SerializeField] private RectTransform _fillRect;
        [SerializeField] private RectTransform _handleRect;
        [SerializeField] private Direction _direction = Direction.LeftToRight;
        [SerializeField] protected T _value;
        [SerializeField] protected T _minValue;
        [SerializeField] protected T _maxValue;
        [SerializeField] protected T _step;
        [SerializeField] private UniSigner<T> _onValueChanged = new();

        protected float _normalizedValue;
        private int _axis;
        private bool _reverseValue;

        private RectTransform _thisRectTransform;

        private Image _fillImage;
        private RectTransform _fillContainerRect;

        private RectTransform _handleContainerRect;

        private Vector2 _offset = Vector2.zero; // The offset from handle position to mouse down position

#pragma warning disable 649
        private DrivenRectTransformTracker _tracker;  // field is never assigned warning
#pragma warning restore 649

        #region Abstract
        public abstract T Step { get; set; }
        public abstract float NormalizedValue { get; set; }
        protected abstract void Normalized(T value);
        protected abstract T StepToLeft { get; }
        protected abstract T StepToRight { get; }
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
        public RectTransform HandleRect
        {
            get => _handleRect;
            set
            {
                if (_handleRect != value)
                {
                    _handleRect = value;
                    UpdateHandleRectReferences();
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
                    UpdateDirection(value);
                    UpdateVisuals();
                }
            }
        }
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

        public Unsubscriber AddListener(Action<T> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _value);
        public void RemoveListener(Action<T> action) => _onValueChanged.Remove(action);

        public bool SetMinMax(T min, T max)
        {
            if (min.CompareTo(max) >= 0) return false;
            if (min.Equals(_minValue) & max.Equals(_maxValue)) return true;

            _minValue = min; _maxValue = max;
            UpdateMinMaxDependencies();
            return true;
        }

        protected void Set(T value, bool sendCallback)
        {
            value = ClampValue(value);

            if (!_value.Equals(value))
            {
                _value = value;
                Normalized(value);

                UpdateVisuals();

                if (sendCallback)
                {
                    UISystemProfilerApi.AddMarker("VSlider.value", this);
                    _onValueChanged.Invoke(value);
                }
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
            Step = _step;
            _value = ClampValue(_value);
            Normalized(_value);
            UpdateVisuals();
        }

        private void UpdateFillRectReferences()
        {
            _fillContainerRect = null;
            _fillImage = null;
            if (_fillRect != null & _fillRect != _thisRectTransform && _fillRect.parent != null)
            {
                _fillContainerRect = (RectTransform)_fillRect.parent;
                _fillImage = _fillRect.GetComponent<Image>();
            }
            else
            {
                _fillRect = null;
            }
        }

        private void UpdateHandleRectReferences()
        {
            _handleContainerRect = null;

            if (_handleRect != null & _handleRect != _thisRectTransform && _handleRect.parent != null)
                _handleContainerRect = (RectTransform)_handleRect.parent;
            else
                _handleRect = null;
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

                _fillRect.anchorMin = anchorMin;
                _fillRect.anchorMax = anchorMax;
            }

            if (_handleContainerRect != null)
            {
                _tracker.Add(this, _handleRect, DrivenTransformProperties.Anchors);
                Vector2 anchorMin = Vector2.zero;
                Vector2 anchorMax = Vector2.one;
                anchorMin[_axis] = anchorMax[_axis] = (_reverseValue ? (1f - _normalizedValue) : _normalizedValue);
                _handleRect.anchorMin = anchorMin;
                _handleRect.anchorMax = anchorMax;
            }
        }
        private bool CanDrag(PointerEventData eventData)
        {
            return eventData.button == PointerEventData.InputButton.Left && isActiveAndEnabled && IsInteractable();
        }

        // Update the slider's position based on the mouse.
        private void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            RectTransform clickRect = _handleContainerRect ? _handleContainerRect : _fillContainerRect;
            if (clickRect != null && clickRect.rect.size[_axis] > 0)
            {
                Vector2 position = Vector2.zero;
                if (!MultipleDisplayUtilities.GetRelativeMousePositionForDrag(eventData, ref position)) return;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(clickRect, position, cam, out Vector2 localCursor)) return;

                localCursor -= clickRect.rect.position;

                float val = Mathf.Clamp01((localCursor - _offset)[_axis] / clickRect.rect.size[_axis]);
                NormalizedValue = (_reverseValue ? 1f - val : val);
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
        sealed protected override void Awake()
        {
            base.Awake();
            _thisRectTransform = (RectTransform)transform;
            UpdateFillRectReferences();
            UpdateHandleRectReferences();
        }

        sealed protected override void Start()
        {
            base.Start();
            UpdateDirection(_direction);
            Normalized(_value);
            UpdateVisuals();

            _onValueChanged.Init();
        }

        sealed protected override void OnDisable()
        {
            _tracker.Clear();
            base.OnDisable();
        }

        sealed protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (isActiveAndEnabled) 
                UpdateVisuals();
        }

        sealed public override void OnPointerDown(PointerEventData eventData)
        {
            if (!CanDrag(eventData)) return;

            base.OnPointerDown(eventData);

            _offset = Vector2.zero;
            if (_handleContainerRect != null
            && RectTransformUtility.RectangleContainsScreenPoint(_handleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera)
            && RectTransformUtility.ScreenPointToLocalPointInRectangle(_handleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera,
               out Vector2 localMousePos))
            {
                _offset = localMousePos;
            }
            else
            {
                // Outside the slider handle - jump to this point instead
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (CanDrag(eventData))
                UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        sealed public override void OnMove(AxisEventData eventData)
        {
            if (!isActiveAndEnabled || !IsInteractable())
            {
                base.OnMove(eventData);
                return;
            }

            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    if (_axis == HORIZONTAL && FindSelectableOnLeft() == null)
                        Set(_reverseValue ? StepToRight : StepToLeft, true);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    if (_axis == HORIZONTAL && FindSelectableOnRight() == null)
                        Set(_reverseValue ? StepToLeft : StepToRight, true);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Up:
                    if (_axis == VERTICAL && FindSelectableOnUp() == null)
                        Set(_reverseValue ? StepToLeft : StepToRight, true);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Down:
                    if (_axis == VERTICAL && FindSelectableOnDown() == null)
                        Set(_reverseValue ? StepToRight : StepToLeft, true);
                    else
                        base.OnMove(eventData);
                    break;
            }
        }
        sealed protected override void OnDidApplyAnimationProperties()
        {
            // Has value changed? Various elements of the slider have the old normalisedValue assigned, we can use this to perform a comparison.
            // We also need to ensure the value stays within min/max.
            _value = ClampValue(_value);
            float oldNormalizedValue = _normalizedValue;
            if (_fillContainerRect != null)
            {
                if (_fillImage != null && _fillImage.type == Image.Type.Filled)
                    oldNormalizedValue = _fillImage.fillAmount;
                else
                    oldNormalizedValue = (_reverseValue ? 1 - _fillRect.anchorMin[_axis] : _fillRect.anchorMax[_axis]);
            }
            else if (_handleContainerRect != null)
                oldNormalizedValue = (_reverseValue ? 1 - _handleRect.anchorMin[_axis] : _handleRect.anchorMin[_axis]);

            UpdateVisuals();

            if (oldNormalizedValue != _normalizedValue)
            {
                UISystemProfilerApi.AddMarker("VSlider.value", this);
                _onValueChanged.Invoke(_value);
            }
            // UUM-34170 Apparently, some properties on slider such as IsInteractable and Normalcolor Animation is broken.
            // We need to call base here to render the animation on Scene
            base.OnDidApplyAnimationProperties();
        }
        #endregion

        #region FindSelectable..
        sealed public override Selectable FindSelectableOnLeft()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == HORIZONTAL)
                return null;
            return base.FindSelectableOnLeft();
        }
        sealed public override Selectable FindSelectableOnRight()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == HORIZONTAL)
                return null;
            return base.FindSelectableOnRight();
        }
        sealed public override Selectable FindSelectableOnUp()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == VERTICAL)
                return null;
            return base.FindSelectableOnUp();
        }
        sealed public override Selectable FindSelectableOnDown()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == VERTICAL)
                return null;
            return base.FindSelectableOnDown();
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
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion


#if UNITY_EDITOR

        private bool _delayedUpdateVisuals = false;
        private void Update()
        {
            if (_delayedUpdateVisuals)
            {
                _delayedUpdateVisuals = false;

                _axis = (_direction == Direction.LeftToRight | _direction == Direction.RightToLeft) ? HORIZONTAL : VERTICAL;
                _reverseValue = _direction == Direction.RightToLeft | _direction == Direction.TopToBottom;

                Step = _step;
                _value = ClampValue(_value);
                Normalized(_value);

                if (_thisRectTransform == null)
                    _thisRectTransform = (RectTransform)transform;
                UpdateFillRectReferences();
                UpdateHandleRectReferences();

                UpdateVisuals();
            }
        }
        sealed protected override void OnValidate()
        {
            base.OnValidate();

            if (!Application.isPlaying)
            {
                _delayedUpdateVisuals = isActiveAndEnabled;

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
#endif
    }

}
