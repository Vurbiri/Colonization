using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    sealed public class HelpWindow : ASwitchableWindow
    {
        public override Switcher Init()
        {
            _switcher.Init(GetComponent<HelpToggleGroup>());
            GetComponentInChildren<SimpleButton>().AddListener(_switcher.Close);

            Destroy(this);

            return _switcher;
        }

#if UNITY_EDITOR
        public override void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
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
