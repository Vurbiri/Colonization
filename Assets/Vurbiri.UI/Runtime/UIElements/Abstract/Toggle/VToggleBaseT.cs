using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
	public abstract class VToggleBase<TToggle> : VSelectable, IPointerClickHandler, ISubmitHandler  where TToggle : VToggleBase<TToggle>
    {
        [SerializeField] protected bool _isOn;
        [SerializeField] protected VToggleGroup<TToggle> _group;
        [SerializeField] protected UniSubscription<bool> _onValueChanged = new();

        protected readonly TToggle _this;

        public bool IsOn { get => _isOn; set => SetValue(value, true); }
        public bool SilentIsOn { get => _isOn; set => SetValue(value, false); }
        public VToggleGroup<TToggle> Group
        {
            get => _group;
            set
            {
                if (_group == value) return;

                if (_group != null)
                    _group.UnregisterToggle(_this);

                _group = value;

                if (value != null && isActiveAndEnabled)
                    value.RegisterToggle(_this);

                UpdateVisualInstant();
            }
        }

        protected VToggleBase() : base()
        {
            _this = (TToggle)this;
        }

        protected override void Start()
        {
            base.Start();

            _onValueChanged.Init(_isOn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Unsubscription AddListener(Action<bool> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _isOn);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveListener(Action<bool> action) => _onValueChanged.Remove(action);

        protected abstract void UpdateVisual();
        protected abstract void UpdateVisualInstant();

        public void LeaveGroup()
        {
            if (_group != null)
                _group.UnregisterToggle(_this);
            _group = null;
        }

        internal void SetFromGroup(bool value)
        {
            if (_isOn == value) return;

            _isOn = value;
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                UpdateVisualInstant();
            else
#endif
                UpdateVisual();

            UISystemProfilerApi.AddMarker("VToggle.onValueChanged", _this);
            _onValueChanged.Invoke(_isOn);
        }

        protected void SetValue(bool value, bool sendCallback)
        {
            if (_isOn == value || (_group != null && !_group.CanSetValue(_this, value)))
                return;

            _isOn = value;

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                UpdateVisualInstant();
            else
#endif
                UpdateVisual();

            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("VToggle.onValueChanged", _this);
                _onValueChanged.Invoke(_isOn);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_group != null)
                _group.RegisterToggle(_this);

            UpdateVisualInstant();
        }

        protected override void OnDisable()
        {
            if (_group != null)
                _group.UnregisterToggle(_this);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InternalToggle()
        {
            if (isActiveAndEnabled && IsInteractable())
                SetValue(!_isOn, true);
        }

#if UNITY_EDITOR
        private VToggleGroup<TToggle> _groupEditor;
        private bool _isOnEditor;

        protected override void OnValidate()
        {
            base.OnValidate();

           OnValidateAsync();
        }

        protected virtual async void OnValidateAsync()
        {
            await System.Threading.Tasks.Task.Delay(2);

            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode || this == null || !isActiveAndEnabled)
                return;

            if (_groupEditor != _group)
            {
                if (_groupEditor != null)
                    _groupEditor.UnregisterToggle(_this);

                if (_group != null && isActiveAndEnabled)
                    _group.RegisterToggle(_this);

                _groupEditor = _group;

                UpdateVisualInstant();
            }

            if (_isOnEditor != _isOn)
            {
                _isOn = _isOnEditor;
                SetValue(!_isOn, false);

                _isOnEditor = _isOn;
                UpdateVisualInstant();
            }
        }
#endif
    }
}
