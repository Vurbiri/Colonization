//Assets\Vurbiri.UI\Runtime\VToggle.cs
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Toggle", 30)]
    [RequireComponent(typeof(RectTransform))]
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
        private ITransitionEffect _transitionEffect;
        private TMP_Text _caption;

        #region Properties
        public bool IsOn { get => _isOn; set => Set(value, true); }
        public bool SilentIsOn { get => _isOn; set => Set(value, false); }
        public TMP_Text Caption => _caption;
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
                if (_fadeDuration == value) return;

                _fadeDuration = value;
                _transitionEffect.SetDuration(_fadeDuration);
            }
        }
        public SwitchingType Switching
        {
            get => _switchingType;
            set 
            {
                if (_switchingType == value) return;

                _switchingType = value;
                _transitionEffect = TransitionEffectCreate();
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

            _caption = GetComponentInChildren<TMP_Text>();

            _onValueChanged.Clear();
            _onValueChanged.Add(ProfilerApiAddMarker);
        }
        #endregion

        public Unsubscriber AddListener(Action<bool> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _isOn);
        public void RemoveListener(Action<bool> action) => _onValueChanged.Remove(action);

        public void SetColors(Color colorOn, Color colorOff)
        {
            if(colorOn == _colorOn & colorOff == _colorOff) return;
            
            _colorOn = colorOn; _colorOff = colorOff;

            _transitionEffect.ColorsUpdate();
            _transitionEffect.PlayInstant(_isOn);
        }

        internal void SetFromGroup(bool value)
        {
            if (_isOn == value) return;

            _isOn = value;
            _transitionEffect.Play(_isOn);

            _onValueChanged.Invoke(_isOn);
        }

        private void Set(bool value, bool sendCallback)
        {
            if (_isOn == value) return;

            if (_group != null && !_group.TrySetValue(this, value))
                return;

            _isOn = value;
            _transitionEffect.Play(_isOn);

            if (sendCallback) _onValueChanged.Invoke(_isOn);
        }

        private void InternalToggle()
        {
            if (!isActiveAndEnabled || !IsInteractable())
                return;

            Set(!_isOn, true);
        }

        #region Calls
        protected override void OnStateTransition(int intState, Color targetColor, float duration)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { _transitionEffect ??= TransitionEffectCreate(); _stateFilterOn = _checkmarkOn != null; _stateFilterOff = _checkmarkOff != null; }
#endif
            if (_stateFilterOn[intState]) _transitionEffect.StateTransitionOn(targetColor, duration);
            if (_stateFilterOff[intState]) _transitionEffect.StateTransitionOff(targetColor, duration);
        }

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
                    Set(!_isOn, true);
                }
            }

            base.OnDidApplyAnimationProperties();
        }
        #endregion

        private ITransitionEffect TransitionEffectCreate()
        {
            if (_checkmarkOn != null) _checkmarkOn.canvasRenderer.SetAlpha(0f);
            if (_checkmarkOff != null) _checkmarkOff.canvasRenderer.SetAlpha(0f);

            if (_switchingType == SwitchingType.OnOffCheckmark && OnOffEffect.Validate(this))
                return new OnOffEffect(_fadeDuration, _isOn, _checkmarkOn);

            if (_switchingType == SwitchingType.SwitchCheckmark && SwitchEffect.Validate(this))
                return new SwitchEffect(this);

            if (_switchingType == SwitchingType.ColorCheckmark && ColorEffect.Validate(this))
                return new ColorEffect(this);

            return new EmptyEffect();
        }

        private void ProfilerApiAddMarker(bool b) => UISystemProfilerApi.AddMarker("VToggle.onValueChanged", this);

        #region ICanvasElement
        public void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                _onValueChanged.Invoke(_isOn);
#endif
        }
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!Application.isPlaying)
            {
                _transitionEffect ??= TransitionEffectCreate();

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
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
