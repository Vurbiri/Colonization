using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEdificeButton : APanelButton
    {
        public virtual void Init(Crossroad crossroad, InputController inputController, int index, Sprite sprite, bool isOn)
        {
            _canvasGroup.alpha = _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            transform.localPosition = Offset * index;

            Attach(crossroad, sprite);
            InitClick(inputController);

            if (isOn) Enable();
        }

        public abstract void OnChange(Crossroad crossroad, Sprite sprite);
    }
}
