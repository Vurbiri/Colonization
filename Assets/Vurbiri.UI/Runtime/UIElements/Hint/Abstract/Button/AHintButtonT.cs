using UnityEngine;
using UnityEngine.EventSystems;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public abstract class AHintButton<TValue> : AVButton<TValue>
    {
        private Id<HintId> _hint;
        private bool _isShowingHint = false;
        private HintOffset _hintOffset;

        protected string _hintText;

        [Impl(256)]
        protected void InternalInit(Id<HintId> hint, float heightRatio)
        {
            _hint = hint;
            _hintOffset = _thisRectTransform.GetHintOffset(heightRatio);
        }

        sealed public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_hintText, _thisRectTransform, _hintOffset);
        }
        sealed public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Hide();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            Hide();
        }

        [Impl(256)]
        private void Hide()
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }
    }
}
