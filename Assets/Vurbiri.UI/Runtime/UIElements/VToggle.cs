//Assets\Vurbiri.UI\Runtime\UIElements\VToggle.cs
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.TOGGLE, VUI_CONST_ED.TOGGLE_ORDER)]
    [RequireComponent(typeof(RectTransform))]
#endif
    sealed public partial class VToggle : VSelectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {
        [SerializeField] private bool _isOn;
        [SerializeField] private float _fadeDuration = 0.125f;
        [SerializeField] private SwitchingType _switchingType;
        [SerializeField] private Graphic _checkmarkOn;
        [SerializeField] private Graphic _checkmarkOff;
        [SerializeField] private Color _colorOn = Color.green;
        [SerializeField] private Color _colorOff = Color.red;
        [SerializeField] private VToggleGroup _group;
        [SerializeField] private UniSigner<bool> _onValueChanged = new();

        private EnumFlags<SelectionState> _stateFilterOn = false, _stateFilterOff = false;
        private ITransitionEffect _transitionEffect = new EmptyEffect();

        #region Properties
        public bool IsOn { get => _isOn; set => SetValue(value, true); }
        public bool SilentIsOn { get => _isOn; set => SetValue(value, false); }
        public VToggleGroup Group
        {
            get => _group;
            set 
            {
                if (_group == value) return;
                
                if (_group != null)
                    _group.UnregisterToggle(this);

                _group = value;

                if (value != null && isActiveAndEnabled)
                    value.RegisterToggle(this);

                _transitionEffect.PlayInstant(_isOn);
            }
        }
        public float CheckmarkFadeDuration
        {
            get => _fadeDuration;
            set
            {
                if (_fadeDuration != value)
                {
                    _fadeDuration = value;
                    _transitionEffect.SetDuration(_fadeDuration);
                }
            }
        }
        public SwitchingType Switching
        {
            get => _switchingType;
            set 
            {
                if (_switchingType != value)
                {
                    _switchingType = value;
                    _transitionEffect = TransitionEffectCreate();
                }
            }
        }
        public Graphic CheckmarkOn
        {
            get => _checkmarkOn;
            set
            {
                if (_checkmarkOn == value) return;

                _checkmarkOn = value;
                if (!_transitionEffect.SetGraphic(value, _checkmarkOff))
                    _transitionEffect = TransitionEffectCreate();

                _transitionEffect.PlayInstant(_isOn);
            }
        }
        public Graphic CheckmarkOff
        {
            get => _checkmarkOff;
            set
            {
                if (_checkmarkOff == value) return;

                _checkmarkOff = value;
                if (!_transitionEffect.SetGraphic(_checkmarkOn, value))
                    _transitionEffect = TransitionEffectCreate();

                _transitionEffect.PlayInstant(_isOn);
            }
        }
        public Color ColorOn
        {
            get => _colorOn;
            set => SetColors(value, _colorOff);
        }
        public Color ColorOff
        {
            get => _colorOff;
            set => SetColors(_colorOn, value);
        }
        #endregion

        private VToggle() { }

        #region Awake, Start
        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            if (_checkmarkOn != null)
            {
                TargetGraphic current;
                for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                {
                    current = _targetGraphics[i];
                    if (current.IsNotNull & current == _checkmarkOn)
                    {
                        _stateFilterOn = current.Filter;
                        _targetGraphics.RemoveAt(i);
                    }
                    else if (current.IsNotNull & current == _checkmarkOff)
                    {
                        _stateFilterOff = current.Filter;
                        _targetGraphics.RemoveAt(i);
                    }
                }
            }

            _transitionEffect = TransitionEffectCreate();

            base.Awake();
        }
        protected override void Start()
        {
            base.Start();

            _onValueChanged.Init();
        }
        #endregion

        public Unsubscriber AddListener(Action<bool> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _isOn);
        public void RemoveListener(Action<bool> action) => _onValueChanged.Remove(action);

        public void SetColors(Color colorOn, Color colorOff)
        {
            if(colorOn == _colorOn & colorOff == _colorOff) return;
            
            _colorOn = colorOn; _colorOff = colorOff;

            _transitionEffect.ColorsUpdate(colorOn, colorOff);
            _transitionEffect.PlayInstant(_isOn);
        }

        internal void SetFromGroup(bool value)
        {
            if (_isOn == value) return;

            _isOn = value;
            _transitionEffect.Play(_isOn);

            UISystemProfilerApi.AddMarker("VToggle.onValueChanged", this);
            _onValueChanged.Invoke(_isOn);
        }

        private void SetValue(bool value, bool sendCallback)
        {
            if (_isOn == value || (_group != null && !_group.CanSetValue(this, value))) 
                return;

            _isOn = value;
            _transitionEffect.Play(_isOn);

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("VToggle.onValueChanged", this);
                _onValueChanged.Invoke(_isOn);
            }
        }

        protected override void StartScaleTween(Vector3 targetScale, float duration)
        {
            base.StartScaleTween(targetScale, duration);

            _transitionEffect.StateTransitionClear();
        }
        protected override void StartColorTween(int intState, Vector3 targetScale, Color targetColor, float duration)
        {
            base.StartColorTween(intState, targetScale, targetColor, duration);

            if (_stateFilterOn[intState]) _transitionEffect.StateTransitionOn(targetColor, duration);
            if (_stateFilterOff[intState]) _transitionEffect.StateTransitionOff(targetColor, duration);
        }
        protected override void DoSpriteSwap(Vector3 targetScale, Sprite targetSprite, float duration)
        {
            base.DoSpriteSwap(targetScale, targetSprite, duration);

            _transitionEffect.StateTransitionClear();
        }

        private void InternalToggle()
        {
            if (!isActiveAndEnabled || !IsInteractable())
                return;

            SetValue(!_isOn, true);
        }

        #region Calls

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_group != null)
                _group.RegisterToggle(this);

            _transitionEffect.PlayInstant(_isOn);
        }

        protected override void OnDisable()
        {
            if (_group != null)
                _group.UnregisterToggle(this);

            _transitionEffect.Stop();

            base.OnDisable();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                InternalToggle();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            // Check if isOn has been changed by the animation. Unfortunately there is no way to check if we don't have a graphic.
            if (_transitionEffect.IsValid)
            {
                bool oldValue = _transitionEffect.Value;
                if (_isOn != oldValue)
                {
                    _isOn = oldValue;
                    SetValue(!_isOn, true);
                }
            }

            base.OnDidApplyAnimationProperties();
        }
        #endregion

        private ITransitionEffect TransitionEffectCreate()
        {
            if (_checkmarkOn != null) _checkmarkOn.canvasRenderer.SetAlpha(0f);
            if (_checkmarkOff != null) _checkmarkOff.canvasRenderer.SetAlpha(0f);

            if (_switchingType == SwitchingType.OnOffCheckmark && _checkmarkOn != null)
                return new OnOffEffect(_fadeDuration, _isOn, _checkmarkOn);

            if (_switchingType == SwitchingType.SwitchCheckmark && _checkmarkOn != null & _checkmarkOff != null)
                return new SwitchEffect(_fadeDuration, _isOn, _checkmarkOn, _checkmarkOff);

            if (_switchingType == SwitchingType.ColorCheckmark && _checkmarkOn != null)
                return new ColorEffect(_fadeDuration, _isOn, _checkmarkOn, _colorOn, _colorOff);

            return new EmptyEffect();
        }

        #region ICanvasElement
        public void Rebuild(CanvasUpdate executing) { }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (!Application.isPlaying)
            {
                _transitionEffect = TransitionEffectCreate();
                _stateFilterOn = _checkmarkOn != null;
                _stateFilterOff = _checkmarkOff != null;
            }

            base.OnValidate();
        }
#endif

        #region Nested: SwitchingType
        //***********************************
        public enum SwitchingType
        {
            OnOffCheckmark,
            SwitchCheckmark,
            ColorCheckmark
        }
        #endregion
    }
}
