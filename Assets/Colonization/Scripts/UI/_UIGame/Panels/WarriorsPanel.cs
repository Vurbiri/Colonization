//Assets\Colonization\Scripts\UI\_UIGame\Panels\WarriorsPanel.cs
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public class WarriorsPanel : MonoBehaviour
	{
        [SerializeField] private VToggle _toggle;
        [SerializeField] private CurrentMax _widget;

        public void Init(Human player, ProjectColors colors)
		{
           var warriors = player.Warriors;

            warriors.CountReactive.Subscribe(count => _toggle.interactable = count > 0);
            _widget.Init(warriors.CountReactive, player.GetAbility(HumanAbilityId.MaxWarrior), colors);

            Destroy(this);
        }


#if UNITY_EDITOR

        public RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.BackgroundPanel;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _toggle.CheckmarkOn.color = colors.BackgroundPanel;
            _toggle.CheckmarkOff.color = colors.BackgroundPanel;

            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = _widget.Size + padding * 2f;

            return thisRectTransform;
        }

        private void OnValidate()
        {
            if(_toggle == null)
                _toggle = GetComponent<VToggle>();
            if(_widget == null)
                _widget = GetComponentInChildren<CurrentMax>();
        }
#endif
	}
}
