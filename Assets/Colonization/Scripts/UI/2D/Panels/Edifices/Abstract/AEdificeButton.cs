using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEdificeButton : APanelButton
    {
        public virtual void Init(Crossroad crossroad, int index, Sprite sprite, bool isOn)
        {
            _canvasGroup.alpha = _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            transform.localPosition = Offset * index;

            Attach(crossroad, sprite);
            InitClick();

            if (isOn) Enable();
        }

        public abstract void OnChange(Crossroad crossroad, Sprite sprite);
    }
}
