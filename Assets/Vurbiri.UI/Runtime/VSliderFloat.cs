//Assets\Vurbiri.UI\Runtime\VSlider.cs
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Slider Float", 34)]
    [RequireComponent(typeof(RectTransform))]
    public class VSliderFloat : VSelectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
    {
        private const int HORIZONTAL = 0, VERTICAL = 1;
        public const float RATE_STEP_MIN = 0.025f, RATE_STEP_MAX = 0.25f;

        [SerializeField] private RectTransform _fillRect;
        [SerializeField] private RectTransform _handleRect;
        [SerializeField] private Direction _direction = Direction.LeftToRight;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue = 1f;
        [SerializeField] private float _step = 0.1f;
        [SerializeField] private float _value;
        [SerializeField] private UniSigner<float> _onValueChanged = new();

        private float _normalizedValue;
        private int _axis;
        private bool _reverseValue;

        private RectTransform _thisRectTransform;

        private Image _fillImage;
        private Transform _fillTransform;
        private RectTransform _fillContainerRect;
        private Transform _handleTransform;
        private RectTransform _handleContainerRect;

        private Vector2 _offset = Vector2.zero; // The offset from handle position to mouse down position

#pragma warning disable 649
        private DrivenRectTransformTracker _tracker;  // field is never assigned warning
#pragma warning restore 649

        #region Properties
        public virtual float Value
        {
            get => _value;
            set 
            { 
                if (Set(value, true))
                    UpdateVisuals(); 
            }
        }
        public virtual float SilentValue
        {
            get => _value;
            set 
            { 
                if (Set(value, false)) 
                    UpdateVisuals(); 
            }
        }
        public float NormalizedValue
        {
            get => _normalizedValue;
            set => this.Value = Mathf.Lerp(_minValue, _maxValue, value);
        }
        public float MinValue 
        { 
            get => _minValue;  
            set 
            {
                if (_minValue == value | value >= _maxValue) return;
                
                _minValue = value;
                UpdateMinMaxDependencies();
            } 
        }
        public float MaxValue
        {
            get => _maxValue;
            set
            {
                if (_maxValue == value | value <= _minValue) return;

                _maxValue = value;
                UpdateMinMaxDependencies();
            }
        }
        public float Step
        {
            get => _step;
            set
            {
                float delta = _maxValue - _minValue;
                _step = Mathf.Clamp(value, delta * RATE_STEP_MIN, delta * RATE_STEP_MAX);
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
        public RectTransform HandleRect
        {
            get => _handleRect;
            set
            {
                if (_handleRect == value) return;

                _handleRect = value;
                UpdateHandleRectReferences();
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
        public ISigner<float> OnValueChanged => _onValueChanged;
        #endregion

        protected VSliderFloat() { }

        public virtual void SetValueWithoutNotify(float input)
        {
            if(Set(input, false)) 
                UpdateVisuals();
        }

        public Unsubscriber AddListener(Action<float> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _value);
        public void RemoveListener(Action<float> action) => _onValueChanged.Remove(action);

        public bool SetMinMax(float min, float max)
        {
            if(min >= max) return false;
            if(min == _minValue & max == _maxValue) return true;

            _maxValue = min; _maxValue = max;
            UpdateMinMaxDependencies();
            return true;
        }

        private bool Set(float value, bool sendCallback)
        {
            value = Mathf.Clamp(value, _minValue, _maxValue);
            _normalizedValue = Mathf.InverseLerp(_minValue, _maxValue, value);

            if (_value == value) return false;

            _value = value;

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Slider.value", this);
                _onValueChanged.Invoke(value);
            }
            return true;
        }

        private void UpdateMinMaxDependencies()
        {
            float delta = _maxValue - _minValue;
            _step = Mathf.Clamp(_step, delta * RATE_STEP_MIN, delta * RATE_STEP_MAX);
            Set(_value, true);
            UpdateVisuals();
        }

        private void UpdateFillRectReferences()
        {
            if (_fillRect && _fillRect != _thisRectTransform)
            {
                _fillTransform = _fillRect.transform;
                _fillImage = _fillRect.GetComponent<Image>();
                if (_fillTransform.parent != null)
                    _fillContainerRect = _fillTransform.parent.GetComponent<RectTransform>();
            }
            else
            {
                _fillRect = null;
                _fillContainerRect = null;
                _fillImage = null;
            }
        }

        private void UpdateHandleRectReferences()
        {
            if (_handleRect && _handleRect != _thisRectTransform)
            {
                _handleTransform = _handleRect.transform;
                if (_handleTransform.parent != null)
                    _handleContainerRect = _handleTransform.parent.GetComponent<RectTransform>();
            }
            else
            {
                _handleRect = null;
                _handleContainerRect = null;
            }
        }

        // Force-update the slider. Useful if you've changed the properties and want it to update visually.
        private void UpdateVisuals()
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

        // Update the slider's position based on the mouse.
        private void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            RectTransform clickRect = _handleContainerRect ?? _fillContainerRect;
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

            if (_fillImage != null && _fillImage.type == Image.Type.Filled)
            {
                _fillImage.fillMethod = _axis == HORIZONTAL ? Image.FillMethod.Horizontal : Image.FillMethod.Vertical;
                _fillImage.fillOrigin = _reverseValue ? 1 : 0;
            }
        }

        private bool CanDrag(PointerEventData eventData)
        {
            return isActiveAndEnabled && IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
        }

        #region Calls

        protected override void Awake()
        {
            base.Awake();
            _thisRectTransform = (RectTransform)transform;
            UpdateFillRectReferences();
            UpdateHandleRectReferences();
        }

        protected override void Start()
        {
            base.Start();
            UpdateDirection(_direction);
            Set(_value, false);
            UpdateVisuals();

            _onValueChanged.Clear();
        }

        protected override void OnDisable()
        {
            _tracker.Clear();
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            //This can be invoked before OnEnabled is called. So we shouldn't be accessing other objects, before OnEnable is called.
            if (isActiveAndEnabled) UpdateVisuals();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!CanDrag(eventData)) return;

            base.OnPointerDown(eventData);

            _offset = Vector2.zero;
            if (_handleContainerRect != null 
            && RectTransformUtility.RectangleContainsScreenPoint(_handleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera)
            && RectTransformUtility.ScreenPointToLocalPointInRectangle(_handleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out Vector2 localMousePos))
            {
                    _offset = localMousePos;
            }
            else
            {
                // Outside the slider handle - jump to this point instead
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (CanDrag(eventData))
                UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public override void OnMove(AxisEventData eventData)
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
                        Set(_reverseValue ? _value + _step : _value - _step, true);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Right:
                    if (_axis == HORIZONTAL && FindSelectableOnRight() == null)
                        Set(_reverseValue ? _value - _step : _value + _step, true);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Up:
                    if (_axis == VERTICAL && FindSelectableOnUp() == null)
                        Set(_reverseValue ? _value - _step : _value + _step, true);
                    else
                        base.OnMove(eventData);
                    break;
                case MoveDirection.Down:
                    if (_axis == VERTICAL && FindSelectableOnDown() == null)
                        Set(_reverseValue ? _value + _step : _value - _step, true);
                    else
                        base.OnMove(eventData);
                    break;
            }
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        protected override void OnDidApplyAnimationProperties()
        {
            // Has value changed? Various elements of the slider have the old normalisedValue assigned, we can use this to perform a comparison.
            // We also need to ensure the value stays within min/max.
            _value = Mathf.Clamp(_value, _minValue, _maxValue);
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
                UISystemProfilerApi.AddMarker("Slider.value", this);
                _onValueChanged.Invoke(_value);
            }
            // UUM-34170 Apparently, some properties on slider such as IsInteractable and Normalcolor Animation is broken.
            // We need to call base here to render the animation on Scene
            base.OnDidApplyAnimationProperties();
        }
        #endregion

        #region FindSelectable..
        public override Selectable FindSelectableOnLeft()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == HORIZONTAL)
                return null;
            return base.FindSelectableOnLeft();
        }
        public override Selectable FindSelectableOnRight()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == HORIZONTAL)
                return null;
            return base.FindSelectableOnRight();
        }
        public override Selectable FindSelectableOnUp()
        {
            if (navigation.mode == Navigation.Mode.Automatic && _axis == VERTICAL)
                return null;
            return base.FindSelectableOnUp();
        }
        public override Selectable FindSelectableOnDown()
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

        private void Update_Editor()
        {
            if (!Application.isPlaying)
            {
                _axis = (_direction == Direction.LeftToRight | _direction == Direction.RightToLeft) ? HORIZONTAL : VERTICAL;
                _reverseValue = _direction == Direction.RightToLeft | _direction == Direction.TopToBottom;

                if(_thisRectTransform == null)
                _thisRectTransform = (RectTransform)transform;
                UpdateFillRectReferences();
                UpdateHandleRectReferences();

                if (_fillImage != null && _fillImage.type == Image.Type.Filled)
                {
                    _fillImage.fillMethod = _axis == HORIZONTAL ? Image.FillMethod.Horizontal : Image.FillMethod.Vertical;
                    _fillImage.fillOrigin = _reverseValue ? 1 : 0;
                }
            }
        }
        protected override void OnValidate()
        {
            base.OnValidate();

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
