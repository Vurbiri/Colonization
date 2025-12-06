using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(ScrollRect))]
    public class HelpWindow : VToggleGroup<ToggleHelp>
    {
        [SerializeField] private Switcher _switcher;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private SimpleButton _closeButton;

        public Switcher Init()
        {
            _switcher.Init(this);

            _closeButton.AddListener(_switcher.Close);
            _closeButton = null;

            _onValueChanged.Add(OnValueChanged);

            return _switcher;
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
            
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            _switcher ??= new();
            _switcher.OnValidate(this);

            this.SetChildren(ref _scroll);
            this.SetChildren(ref _closeButton);
        }

        public void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            var color = colors.PanelBack.SetAlpha(1f);

            var image = GetComponent<UnityEngine.UI.Image>();
            image.color = color;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            image = this.GetComponentInChildren<UnityEngine.UI.Image>("TogglesBack");
            image.color = color;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;
        }
#endif
    }
}
