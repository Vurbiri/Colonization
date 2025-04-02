//Assets\Vurbiri.UI\Runtime\VToggleGroup.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
    [AddComponentMenu(VUI_CONST.NAME_MENU + "Toggle Group", 31), DisallowMultipleComponent]
    sealed public class VToggleGroup : UIBehaviour
    {
        [SerializeField] private bool _allowSwitchOff = false;

        private readonly List<VToggle> _toggles = new();
        private VToggle _activeToggle;

        public bool allowSwitchOff 
        { 
            get => _allowSwitchOff; 
            set
            {
                if (value == _allowSwitchOff) return;
                
                if (!value & _toggles.Count > 0 && _activeToggle == null)
                {
                    _activeToggle = _toggles[0];
                    _activeToggle.SetFromGroup(true);
                }

                _allowSwitchOff = value;
            }
        }

        public bool IsActiveToggle => _activeToggle != null;
        public VToggle ActiveToggle => _activeToggle;

        private VToggleGroup() { }

        public void SetAllTogglesOff()
        {
            if (_activeToggle == null) return;

            _allowSwitchOff = true;
            _activeToggle.SetFromGroup(false);
            _activeToggle = null;
        }

        internal void RegisterToggle(VToggle toggle)
        {
            if (_toggles.Contains(toggle)) return;

            _toggles.Add(toggle);

            if (!isActiveAndEnabled) return;

            if (!_allowSwitchOff & _activeToggle == null)
            {
                toggle.SetFromGroup(true);
                _activeToggle = toggle;
                return;
            }

            if(toggle.isOn & _activeToggle != null)
                toggle.SetFromGroup(false);
        }

        internal void UnregisterToggle(VToggle toggle)
        {
            if (!_toggles.Remove(toggle) | !isActiveAndEnabled | _activeToggle != toggle) 
                return;

            _activeToggle = null;
            if (!_allowSwitchOff & _toggles.Count > 0)
            {
                _toggles[0].SetFromGroup(true);
                _activeToggle = _toggles[0];
            }
        }

        internal bool TrySetValue(VToggle toggle, bool value)
        {
            if (!isActiveAndEnabled || !toggle.isActiveAndEnabled)
                return true;

            if (value)
            {
                if (_activeToggle != null)
                    _activeToggle.SetFromGroup(false);

                _activeToggle = toggle;
                return true;
            }

            if (_activeToggle != toggle) return true;
            if (!_allowSwitchOff) return false;

            _activeToggle = null;
            return true;
        }

        protected override void OnDisable()
        {
            _activeToggle = null;
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            int count = _toggles.Count;
            _activeToggle = null;

            if (count == 0) return;

            int index;
            for (index = 0; index < count; index++)
            {
                if (_toggles[index].isOn)
                {
                    _activeToggle = _toggles[index++];
                    break;
                }
            }

            if (_activeToggle == null)
            {
                if (!_allowSwitchOff)
                {
                    _activeToggle = _toggles[0];
                    _activeToggle.SetFromGroup(true);
                }
                return;
            }

            for (; index < count; index++)
                _toggles[index].SetFromGroup(false);
        }

#if UNITY_EDITOR
        public bool CheckValue_Editor(VToggle toggle, bool value)
        {
            if (!isActiveAndEnabled)
            {
                _activeToggle = null;
                return value;
            }

            if (_activeToggle == null & !_allowSwitchOff)
                value = true;

            if (!value)
            {
                if (_activeToggle != toggle) return false;
                if (!_allowSwitchOff) return true;

                _activeToggle = null;
                return false;
            }

            foreach(var t in _toggles)
                if (t != toggle) t.SetFromGroup(false);

            _activeToggle = toggle;
            return true;
        }
#endif
    }
}
