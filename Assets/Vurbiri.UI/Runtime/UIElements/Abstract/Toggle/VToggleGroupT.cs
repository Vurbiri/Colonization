using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public abstract class VToggleGroup<TToggle> : MonoBehaviour where TToggle : VToggleBase<TToggle>
    {
        [SerializeField] protected bool _allowSwitchOff = false;
        [SerializeField] protected UniSubscription<TToggle> _onValueChanged = new();

        protected readonly HashSet<TToggle> _toggles = new();
        protected TToggle _activeToggle;

        public bool AllowSwitchOff
        {
            get => _allowSwitchOff;
            set
            {
                if (value == _allowSwitchOff) return;

                if (!value & _toggles.Count > 0 && _activeToggle == null)
                {
                    ActivateToggle(_toggles.Any());
                }

                _allowSwitchOff = value;
            }
        }

        public bool IsActiveToggle => _activeToggle != null;
        public TToggle ActiveToggle => _activeToggle;

        protected VToggleGroup() { }

        public Unsubscription AddListener(Action<TToggle> action, bool instantGetValue = true) => _onValueChanged.Add(action, instantGetValue, _activeToggle);
        public void RemoveListener(Action<TToggle> action) => _onValueChanged.Remove(action);

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAllTogglesOff()
        {
            if (_activeToggle != null)
            {
                _allowSwitchOff = true;
                _activeToggle.SetFromGroup(false);
                SetToggle(null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetToggle(TToggle toggle)
        {
            _activeToggle = toggle;
            _onValueChanged.Invoke(toggle);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ActivateToggle(TToggle toggle)
        {
            _activeToggle = toggle;
            _activeToggle.SetFromGroup(true);
            _onValueChanged.Invoke(toggle);
        }

        internal void RegisterToggle(TToggle toggle)
        {
            if (_toggles.Add(toggle) && isActiveAndEnabled)
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
            if (!_toggles.Remove(toggle) || !isActiveAndEnabled | _activeToggle != toggle)
                return;

            if (!_allowSwitchOff & _toggles.Count > 0)
            {
                ActivateToggle(_toggles.Any());
            }
            else
            {
                SetToggle(null);
            }
        }

        internal bool CanSetValue(TToggle toggle, bool isOn)
        {
            if (!(isActiveAndEnabled & toggle.isActiveAndEnabled))
                return true;

            if (isOn)
            {
                if (_activeToggle != toggle)
                {
                    if (_activeToggle != null)
                        _activeToggle.SetFromGroup(false);

                    SetToggle(toggle);
                }
                return true;
            }

            if (_activeToggle == toggle)
            {
                if (!_allowSwitchOff)
                    return false;

                SetToggle(null);
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

            if (_toggles.Count == 0) return;

            using IEnumerator<TToggle> enumerator = _toggles.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.IsOn)
                {
                    SetToggle(enumerator.Current);
                    break;
                }
            }

            if (_activeToggle == null)
            {
                if (!_allowSwitchOff)
                {
                    enumerator.Reset(); enumerator.MoveNext();
                    ActivateToggle(enumerator.Current);
                }
                return;
            }

            while (enumerator.MoveNext())
                enumerator.Current.SetFromGroup(false);
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
