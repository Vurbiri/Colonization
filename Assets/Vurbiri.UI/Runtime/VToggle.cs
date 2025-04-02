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
        [SerializeField] private ToggleTransition _markTransition = ToggleTransition.Fade;
        [SerializeField] private float _transitionDuration = 0.2f;
        [SerializeField] private TransitionType _transitionType;
        [SerializeField] private Graphic _checkmarkOn;
        [SerializeField] private Graphic _checkmarkOff;
        [SerializeField] private Color _colorOn = Color.green;
        [SerializeField] private Color _colorOff = Color.red;
        [SerializeField] private VToggleGroup _group;
        [SerializeField] private UnitySubscriber<bool> _onValueChanged = new();

        private bool _isTransitionOn = false, _isTransitionOff = false;
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

                _transitionEffect.PlayInstant();
            }
        }
        public ToggleTransition MarkTransition
        {
            get => _markTransition;
            set
            {
                if (_markTransition == value) return;
                
                _markTransition = value;
                _transitionEffect.TransitionUpdate();
                if (!_transitionEffect.IsValid)
                    _transitionEffect = TransitionEffect.Create(this);
            }
        }
        public float MarkTransitionDuration
        {
            get => _transitionDuration;
            set
            {
                if (_transitionDuration == value) return;

                _transitionDuration = value;
                _transitionEffect.TransitionUpdate();
            }
        }
        public TransitionType MarkTransitionType
        {
            get => _transitionType;
            set 
            {
                if (_transitionType == value) return;

                _transitionType = value;
                _transitionEffect = TransitionEffect.Create(this);
            }
        }
        public Graphic CheckmarkOn
        {
            get => _checkmarkOn;
            set
            {
                if (_checkmarkOn == value) return;

                _checkmarkOn = value;
                if (!_transitionEffect.GraphicUpdate())
                    _transitionEffect = TransitionEffect.Create(this);

                _transitionEffect.PlayInstant();
            }
        }
        public Graphic CheckmarkOff
        {
            get => _checkmarkOff;
            set
            {
                if (_checkmarkOff == value) return;

                _checkmarkOff = value;
                if (!_transitionEffect.GraphicUpdate())
                    _transitionEffect = TransitionEffect.Create(this);

                _transitionEffect.PlayInstant();
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
                Graphic current;
                for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                {
                    current = _targetGraphics[i];
                    if (current != null & current == _checkmarkOn)
                    {
                        _isTransitionOn = true;
                        _targetGraphics.RemoveAt(i);
                    }
                    else if (current != null & current == _checkmarkOff)
                    {
                        _isTransitionOff = true;
                        _targetGraphics.RemoveAt(i);
                    }
                }
            }

            _transitionEffect = TransitionEffect.Create(this);

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
            _transitionEffect.PlayInstant();
        }

        internal void SetFromGroup(bool value)
        {
            if (_isOn == value) return;

            _isOn = value;
            _transitionEffect.Play();

            _onValueChanged.Invoke(_isOn);
        }

        private void Set(bool value, bool sendCallback)
        {
            if (_isOn == value) return;

            if (_group != null && !_group.TrySetValue(this, value))
                return;

            _isOn = value;
            _transitionEffect.Play();

            if (sendCallback) _onValueChanged.Invoke(_isOn);
        }

        private void InternalToggle()
        {
            if (!isActiveAndEnabled || !IsInteractable())
                return;

            Set(!_isOn, true);
        }

        protected override void OnStateTransition(SelectionState state, Color targetColor, float duration, bool instant)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { _transitionEffect ??= TransitionEffect.Create(this); _isTransitionOn = _checkmarkOn; _isTransitionOff = _checkmarkOn; }
#endif

            if (state == SelectionState.Pressed) return;

            if (_isTransitionOn) _transitionEffect.StateTransitionOn(targetColor, duration, instant);
            if (_isTransitionOff) _transitionEffect.StateTransitionOff(targetColor, duration, instant);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_group != null)
                _group.RegisterToggle(this);

            _transitionEffect.PlayInstant();
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
                _transitionEffect ??= TransitionEffect.Create(this);

                if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
                    CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
#endif
        #endregion

        #region Nested: ToggleTransition, TransitionType
        //***********************************
        public enum ToggleTransition
        {
            /// <summary>
            /// Show / hide the toggle instantly
            /// </summary>
            Instant,

            /// <summary>
            /// Fade the toggle in / out smoothly.
            /// </summary>
            Fade
        }
        //***********************************
        public enum TransitionType
        {
            OnOffCheckmark,
            SwitchCheckmark,
            ColorCheckmark
        }
        #endregion
    }
}
