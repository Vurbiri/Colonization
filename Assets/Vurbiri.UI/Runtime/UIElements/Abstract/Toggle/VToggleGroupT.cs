using System;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    public abstract class VToggleGroup<TToggle> : MonoBehaviour where TToggle : VToggleBase<TToggle>
    {
        [SerializeField] protected bool _allowSwitchOff = false;
        [SerializeField] protected UVAction<TToggle> _onValueChanged = new();

        protected readonly List<TToggle> _toggles = new();
        protected TToggle _activeToggle;

        public bool AllowSwitchOff
        {
            [Impl(256)]
            get => _allowSwitchOff;
            set
            {
                if (value == _allowSwitchOff) return;

                if (!value & _toggles.Count > 0 && _activeToggle == null)
                    SetDefaultToggle();

                _allowSwitchOff = value;
            }
        }

        public bool IsActiveToggle { [Impl(256)] get => _activeToggle != null; }
        public TToggle ActiveToggle { [Impl(256)] get => _activeToggle; }

        protected VToggleGroup() { }

        [Impl(256)] public Subscription AddListener(Action<TToggle> action, bool instantGetValue = true) => _onValueChanged.Add(action, _activeToggle, instantGetValue);
        [Impl(256)] public void RemoveListener(Action<TToggle> action) => _onValueChanged.Remove(action);

        [Impl(256)]
        public void ForceOff()
        {
            if (_activeToggle != null)
            {
                _allowSwitchOff = true;
                _activeToggle.SetFromGroup(false);
                SetToggle(null);
            }
        }

        [Impl(256)]
        private void SetToggle(TToggle toggle)
        {
            _activeToggle = toggle;
            _onValueChanged.Invoke(toggle);
        }
        [Impl(256)]
        private void SetToggle(TToggle toggle, bool sendCallback)
        {
            _activeToggle = toggle;
            if (sendCallback)
                _onValueChanged.Invoke(toggle);
        }
        [Impl(256)]
        private void SetDefaultToggle()
        {
            _activeToggle = _toggles[0];
            _activeToggle.SetFromGroup(true);
            _onValueChanged.Invoke(_activeToggle);
        }

        internal void RegisterToggle(TToggle toggle)
        {
            if(_toggles.Contains(toggle)) return;
               
            _toggles.Add(toggle);

            if (isActiveAndEnabled)
            {
                bool isNotActiveToggle = _activeToggle == null;

                if (!_allowSwitchOff & isNotActiveToggle)
                    toggle.SetFromGroup(true);

                if (toggle.IsOn)
                {
                    if (isNotActiveToggle)
                        SetToggle(toggle);
                    else
                        toggle.SetFromGroup(false);
                }
            }
        }

        internal void UnregisterToggle(TToggle toggle)
        {
            if (!_toggles.Remove(toggle) || !isActiveAndEnabled || _activeToggle != toggle)
                return;

            if (!_allowSwitchOff && _toggles.Count > 0)
                SetDefaultToggle();
            else
                SetToggle(null);
        }

        internal bool CanSetValue(TToggle toggle, bool isOn, bool sendCallback)
        {
            if (!(isActiveAndEnabled & toggle.isActiveAndEnabled))
                return true;

            if (isOn)
            {
                if (_activeToggle != toggle)
                {
                    if (_activeToggle != null)
                        _activeToggle.SetFromGroup(false);
                    SetToggle(toggle, sendCallback);
                }
                return true;
            }

            if (_activeToggle == toggle)
            {
                if (!_allowSwitchOff)
                    return false;

                SetToggle(null, sendCallback);
            }
            return true;
        }

        protected virtual void Start()
        {
            _onValueChanged.Init(_activeToggle);
        }

        protected virtual void OnDisable()
        {
            if (_activeToggle != null)
                SetToggle(null);
        }

        protected virtual void OnEnable()
        {
            _activeToggle = null;

            int count = _toggles.Count;
            if (count == 0) return;

            int index = 0;
            for(; index < count; ++index)
            {
                if (_toggles[index].IsOn)
                {
                    SetToggle(_toggles[index]);
                    break;
                }
            }

            if (_activeToggle == null)
            {
                if (!_allowSwitchOff)
                    SetDefaultToggle();
                return;
            }

            for (++index; index < count; ++index)
                _toggles[index].SetFromGroup(false);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (!Application.isPlaying)
            {
                OnEnable();
            }
        }
#endif
    }
}
