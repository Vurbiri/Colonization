using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.UI
{
#if UNITY_EDITOR
    [AddComponentMenu(VUI_CONST_ED.NAME_MENU + VUI_CONST_ED.TOGGLE_GROUP, VUI_CONST_ED.TOGGLE_ORDER), DisallowMultipleComponent]
#endif
    public class VToggleGroup : MonoBehaviour
    {
        [SerializeField] protected bool _allowSwitchOff = false;

        protected readonly HashSet<AVToggle> _toggles = new();
        protected AVToggle _onToggle;

        public bool AllowSwitchOff 
        { 
            get => _allowSwitchOff; 
            set
            {
                if (value == _allowSwitchOff) return;
                
                if (!value & _toggles.Count > 0 && _onToggle == null)
                {
                    _onToggle = _toggles.Any();
                    _onToggle.SetFromGroup(true);
                }

                _allowSwitchOff = value;
            }
        }

        public bool IsActiveToggle => _onToggle != null;
        public AVToggle ActiveToggle => _onToggle;

        protected VToggleGroup() { }

        public void SetAllTogglesOff()
        {
            if (_onToggle == null) return;

            _allowSwitchOff = true;
            _onToggle.SetFromGroup(false);
            _onToggle = null;
        }

        internal void RegisterToggle(AVToggle toggle)
        {
            if (_toggles.Add(toggle))
            {
#if UNITY_EDITOR
                if (!isActiveAndEnabled | !Application.isPlaying) return;
#else
                if (!isActiveAndEnabled) return;
#endif
                bool isNotOnToggle = _onToggle == null;

                if (!_allowSwitchOff & isNotOnToggle)
                    toggle.SetFromGroup(true);

                if (toggle.IsOn)
                {
                    if (isNotOnToggle)
                        _onToggle = toggle;
                    else
                        toggle.SetFromGroup(false);
                }
            }
        }

        internal void UnregisterToggle(AVToggle toggle)
        {
            if (!_toggles.Remove(toggle) || !isActiveAndEnabled | _onToggle != toggle) 
                return;

            _onToggle = null;
            if (!_allowSwitchOff & _toggles.Count > 0)
            {
                _onToggle = _toggles.Any();
                _onToggle.SetFromGroup(true);
            }
        }

        internal bool CanSetValue(AVToggle toggle, bool value)
        {
            if (!(isActiveAndEnabled & toggle.isActiveAndEnabled))
                return true;

            if (value)
            {
                if (_onToggle != null && _onToggle != toggle)
                    _onToggle.SetFromGroup(false);

                _onToggle = toggle;
                return true;
            }

            if (_onToggle == toggle)
            {
                if (!_allowSwitchOff) 
                    return false;

                _onToggle = null;
            }
            return true;
        }

        protected virtual void OnDisable()
        {
            _onToggle = null;
        }

        protected virtual void OnEnable()
        {
            _onToggle = null;

            if (_toggles.Count == 0) return;

            using IEnumerator<AVToggle> enumerator = _toggles.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.IsOn)
                {
                    _onToggle = enumerator.Current;
                    break;
                }
            }

            if (_onToggle == null)
            {
                if (!_allowSwitchOff)
                {
                    enumerator.Reset(); enumerator.MoveNext();
                    _onToggle = enumerator.Current;
                    _onToggle.SetFromGroup(true);
                }
                return;
            }

            while (enumerator.MoveNext())
                enumerator.Current.SetFromGroup(false);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (isActiveAndEnabled && !Application.isPlaying && _toggles.Count > 1)
            {
                if (_onToggle == null)
                {
                    foreach (var toggle in _toggles)
                    {
                        if (toggle.IsOn)
                        {
                            _onToggle = toggle;
                            break;
                        }
                    }
                }

                if (_onToggle == null)
                    return;

                foreach (var toggle in _toggles)
                    if (toggle != _onToggle)
                        toggle.SetFromGroup(false);

            }
        }
#endif
    }
}
