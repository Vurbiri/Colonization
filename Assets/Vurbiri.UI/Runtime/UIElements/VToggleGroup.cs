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

        protected readonly List<AVToggle> _toggles = new();
        protected AVToggle _onToggle;

        public bool AllowSwitchOff 
        { 
            get => _allowSwitchOff; 
            set
            {
                if (value == _allowSwitchOff) return;
                
                if (!value & _toggles.Count > 0 && _onToggle == null)
                {
                    _onToggle = _toggles[0];
                    _onToggle.SetFromGroup(true);
                }

                _allowSwitchOff = value;
            }
        }

        public bool IsActiveToggle => _onToggle != null;
        public AVToggle ActiveToggle => _onToggle;

        private VToggleGroup() { }

        public void SetAllTogglesOff()
        {
            if (_onToggle == null) return;

            _allowSwitchOff = true;
            _onToggle.SetFromGroup(false);
            _onToggle = null;
        }

        internal void RegisterToggle(AVToggle toggle)
        {
            if (_toggles.Contains(toggle)) return;

            _toggles.Add(toggle);

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

        internal void UnregisterToggle(AVToggle toggle)
        {
            if (!_toggles.Remove(toggle) || !isActiveAndEnabled | _onToggle != toggle) 
                return;

            _onToggle = null;
            if (!_allowSwitchOff & _toggles.Count > 0)
            {
                _toggles[0].SetFromGroup(true);
                _onToggle = _toggles[0];
            }
        }

        internal bool CanSetValue(AVToggle toggle, bool value)
        {
            if (!(isActiveAndEnabled & toggle.isActiveAndEnabled))
                return true;

            if (value)
            {
                if (_onToggle != null)
                    _onToggle.SetFromGroup(false);

                _onToggle = toggle;
                return true;
            }

            if (_onToggle != toggle) return true;
            if (!_allowSwitchOff) return false;

            _onToggle = null;
            return true;
        }

        protected virtual void OnDisable()
        {
            _onToggle = null;
        }

        protected virtual void OnEnable()
        {
            int count = _toggles.Count;
            _onToggle = null;

            if (count == 0) return;

            int index;
            for (index = 0; index < count; index++)
            {
                if (_toggles[index].IsOn)
                {
                    _onToggle = _toggles[index++];
                    break;
                }
            }

            if (_onToggle == null)
            {
                if (!_allowSwitchOff)
                {
                    _onToggle = _toggles[0];
                    _onToggle.SetFromGroup(true);
                }
                return;
            }

            for (; index < count; index++)
                _toggles[index].SetFromGroup(false);
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
