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
        [SerializeField] private bool _isFade = true;
        [SerializeField] private float _fadeDuration = 0.15f;
        [SerializeField] private FadeType _fadeType;
        [SerializeField] private Graphic _checkmarkOn;
        [SerializeField] private Graphic _checkmarkOff;
        [SerializeField] private Color _colorOn = Color.green;
        [SerializeField] private Color _colorOff = Color.red;
        [SerializeField] private VToggleGroup _group;
        [SerializeField] private UnitySigner<bool> _onValueChanged = new();

        private TargetGraphic _graphicMarkOn = new(false), _graphicMarkOff = new(false);
        private ITransitionEffect _transitionEffect;
        private TMP_Text _caption;

        #region Properties
        public bool isOn { get => _isOn; set => SetFromGroup(value); }
        public bool isOnSilent { get => _isOn; set => Set(value, false); }
        public TMP_Text caption => _caption;
        public VToggleGroup group
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
        public bool IsCheckmarkFade
        {
            get => _isFade;
            set
            {
                if (_isFade == value) return;
                
                _isFade = value;
                _transitionEffect.TransitionUpdate();
                if (!_transitionEffect.IsValid)
                    _transitionEffect = TransitionEffectCreate();
            }
        }
        public float CheckmarkFadeDuration
        {
            get => _fadeDuration;
            set
            {
                if (_fadeDuration == value) return;

                _fadeDuration = value;
                _transitionEffect.TransitionUpdate();
            }
        }
        public FadeType CheckmarkFadeType
        {
            get => _fadeType;
            set 
            {
                if (_fadeType == value) return;

                _fadeType = value;
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
                    if (current.IsValid & current == _checkmarkOn)
                    {
                        _graphicMarkOn.CopyFlags(current);
                        _targetGraphics.RemoveAt(i);
                    }
                    else if (current.IsValid & current == _checkmarkOff)
                    {
                        _graphicMarkOff.CopyFlags(current);
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

        public Unsubscriber AddListener(Action<bool> action, bool sendCallback = true) => _onValueChanged.Add(action, sendCallback, _isOn);
        public void RemoveListener(Action<bool> action) => _onValueChanged.Remove(action);

        public void SetIsOnWithoutNotify(bool value) => Set(value, false);

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

        protected override void OnStateTransition(int intState, Color targetColor, float duration, bool instant)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { _transitionEffect ??= TransitionEffectCreate(); _graphicMarkOn = _checkmarkOn; _graphicMarkOff = _checkmarkOff; }
#endif
            if (_graphicMarkOn[intState]) _transitionEffect.StateTransitionOn(targetColor, duration, instant);
            if (_graphicMarkOff[intState]) _transitionEffect.StateTransitionOff(targetColor, duration, instant);
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

        private void ProfilerApiAddMarker(bool b) => UISystemProfilerApi.AddMarker("VToggle.onValueChanged", this);

        #region Calls
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                InternalToggle();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            InternalToggle();
        }

        private ITransitionEffect TransitionEffectCreate()
        {
            if (_checkmarkOn != null) _checkmarkOn.canvasRenderer.SetAlpha(0f);
            if (_checkmarkOff != null) _checkmarkOff.canvasRenderer.SetAlpha(0f);

            if (_fadeType == FadeType.OnOffCheckmark && OnOffEffect.Validate(this))
                return new OnOffEffect(this, _isOn, _checkmarkOn);

            /*if (parent._transitionType == TransitionType.SwitchCheckmark && SwitchEffect.Validate(parent))
                return new SwitchEffect(parent);*/

            if (_fadeType == FadeType.ColorCheckmark && ColorEffect.Validate(this))
                return new ColorEffect(this);

            return new EmptyEffect();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            // Check if isOn has been changed by the animation.
            // Unfortunately there is no way to check if we don't have a graphic.
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
        #endregion

        #region Nested: TransitionType
        //***********************************
        public enum FadeType
        {
            OnOffCheckmark,
            SwitchCheckmark,
            ColorCheckmark
        }
        [Flags]
        public enum Side : byte
        {
            None,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8,
        }
        #endregion
    }
}
