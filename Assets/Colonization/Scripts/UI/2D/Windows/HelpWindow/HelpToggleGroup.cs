using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization
{
	sealed public class HelpToggleGroup : AToggleGroup<ToggleHelp>
    {
        [SerializeField] private ScrollRect _scroll;

        protected override void Start()
        {
            base.Start();
            
            _onValueChanged.Add(OnValueChanged);
            Vurbiri.EntryPoint.Transition.OnExit.Add(OnDestroy);
        }

        private void OnValueChanged(ToggleHelp item)
        {
            if (item != null)
            {
                _scroll.content = item.TargetTransform;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = false;

            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                this.SetChildren(ref _scroll);
        }
#endif
    }
}
