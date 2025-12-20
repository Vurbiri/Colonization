#if UNITY_EDITOR
using UnityEngine.UI;
#endif

namespace Vurbiri.Colonization.UI
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
        public void UpdateVisuals_Ed(float pixelsPerUnit, SceneColorsEd colors)
        {
            GetComponent<Image>().SetImageFields(colors.panelBack, pixelsPerUnit);
            this.GetComponentInChildren<Image>("TogglesBack").SetImageFields(colors.panelBack, pixelsPerUnit);

            GetComponentInChildren<SimpleButton>().SetColor_Ed(colors.panelBack);

            foreach (var image in GetComponentInChildren<Scrollbar>().GetComponentsInChildren<Image>())
                image.SetColorField(colors.elements);

            foreach (var toggle in GetComponentsInChildren<ToggleHelp>())
                toggle.SetColors_Ed(colors);
        }
#endif
    }
}
