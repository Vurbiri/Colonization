using UnityEngine;

namespace Vurbiri.UI
{
    sealed public class WorldHint : Hint
    {
        protected override void SetPosition(RectTransform rectTransform, HintOffset offset)
        {
            _backTransform.localPosition = offset.GetOffsetPosition(rectTransform.localPosition, _backTransform.sizeDelta.y * 0.5f);
        }
    }
}
