using TMPro;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	public class PerkTreeProgressBar : MonoBehaviour, IValueId<AbilityTypeId>
    {
        [SerializeField] private Id<AbilityTypeId> _id;
        [SerializeField] private RectTransform _valueRect;
        [SerializeField] private TextMeshProUGUI _valueTMP;

        private readonly float _normalizedPart = 1f / PerkTree.MAX_LEVEL;

        public Id<AbilityTypeId> Id => _id;

        public void Init(RInt progress)
        {
            _valueRect.anchorMin = Vector2.zero;

            progress.Subscribe(OnProgress);
        }

        private void OnProgress(int progress)
        {
            float normalizedValue;

            if (progress >= PerkTree.MAX_PROGRESS)
            {
                normalizedValue = 1f;
                progress = PerkTree.MAX_PROGRESS;
            }
            else
            {
                float level = PerkTree.ProgressToLevel(progress);
                normalizedValue = _normalizedPart * (progress / (2f * (level  + 1f)) + 0.5f * level);
            }

            _valueRect.anchorMax = new(1f, normalizedValue);
            _valueTMP.text = CONST.NUMBERS_STR[progress];
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _valueRect, "BarValue");
            this.SetChildren(ref _valueTMP);

            RectTransform rectTransform = GetComponent<RectTransform>();
            float offset = rectTransform.sizeDelta.x;
            if(_id == AbilityTypeId.Military)
                offset *= -1f;

            rectTransform.anchoredPosition = new Vector2(offset, 0f);
        }
#endif
    }
}
