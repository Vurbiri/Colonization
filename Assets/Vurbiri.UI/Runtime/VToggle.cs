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
    sealed public class VToggle : VSelectable, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {
        [SerializeField] private bool _isOn;
        [SerializeField] private Graphic _markOnGraphic;
        [SerializeField] private VToggleGroup _group;
        [SerializeField] private UnitySubscriber<bool> _onValueChanged = new();
        [SerializeField] private ToggleTransition _toggleTransition = ToggleTransition.Fade;
        
        private TMP_Text _caption;

        public bool isOn { get => _isOn; set => SetFromGroup(value); }
        public bool isOnSilent { get => _isOn; set => Set(value, false); }
        public TMP_Text caption => _caption;
        public VToggleGroup group
        {
            get => _group;
            set 
            {
                if (_group != null)
                    _group.UnregisterToggle(this);

                _group = value;

                if (value != null && isActiveAndEnabled)
                    value.RegisterToggle(this);
                
                PlayEffect(true);
            }
        }

        private VToggle() { }

        #region Calls
        protected override void Start()
        {
            base.Start();

            _caption = GetComponentInChildren<TMP_Text>();

            _onValueChanged.Clear();
            _onValueChanged.Add(ProfilerApiAddMarker);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_group != null) 
                _group.RegisterToggle(this);
            
            PlayEffect(true);
        }

        protected override void OnDisable()
        {
            if (_group != null) 
                _group.UnregisterToggle(this);

            base.OnDisable();
        }
        #endregion

        public Unsubscriber AddListener(Action<bool> action, bool sendCallback = true) => _onValueChanged.Add(action, sendCallback, _isOn);
        public void RemoveListener(Action<bool> action) => _onValueChanged.Remove(action);

        /// <summary>
        /// Set isOn without invoking onValueChanged callback.
        /// </summary>
        /// <param name="value">New Value for isOn.</param>
        public void SetIsOnWithoutNotify(bool value) => Set(value, false);

        internal void SetFromGroup(bool value)
        {
            if (_isOn == value) return;

            _isOn = value;
            PlayEffect(_toggleTransition == ToggleTransition.Instant);

            _onValueChanged.Invoke(_isOn);
        }

        private void Set(bool value, bool sendCallback)
        {
            if (_isOn == value) return;

            if (_group != null && !_group.TrySetValue(this, value))
                return;

            _isOn = value;
            PlayEffect(_toggleTransition == ToggleTransition.Instant);
            
            if (sendCallback) _onValueChanged.Invoke(_isOn);
        }


        // Play the appropriate effect.
        private void PlayEffect(bool instant)
        {
            if (_markOnGraphic == null)
                return;

#if UNITY_EDITOR
            if (!Application.isPlaying) {_markOnGraphic.canvasRenderer.SetAlpha(_isOn ? 1f : 0f); return; }
#endif

            _markOnGraphic.CrossFadeAlpha(_isOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        }

        private void InternalToggle()
        {
            if (!isActiveAndEnabled || !IsInteractable())
                return;

            Set(!_isOn, true);
        }

        private void ProfilerApiAddMarker(bool b) => UISystemProfilerApi.AddMarker("VToggle.onValueChanged", this);

        #region Calls
        protected override void OnDidApplyAnimationProperties()
        {
            // Check if isOn has been changed by the animation.
            // Unfortunately there is no way to check if we don't have a graphic.
            if (_markOnGraphic != null)
            {
                bool oldValue = !Mathf.Approximately(_markOnGraphic.canvasRenderer.GetColor().a, 0);
                if (_isOn != oldValue)
                {
                    _isOn = oldValue;
                    Set(!oldValue, true);
                }
            }

            base.OnDidApplyAnimationProperties();
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

        public void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                _onValueChanged.Invoke(_isOn);
#endif
        }

        #region ICanvasElement
        public void LayoutComplete() { }
        public void GraphicUpdateComplete() { }
        #endregion

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }
#endif
        #endregion

        #region Nested: ToggleTransition
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
        #endregion
    }
}
