using UnityEngine;
using UnityEngine.EventSystems;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public abstract class AHintButton<THint, TValue> : AVButton<TValue> where THint : AHint
    {
        private THint _hint;
        private bool _isShowingHint = false;
        private Vector3 _hintOffset;

        protected string _hintText;

        [Impl(256)]
        protected void InternalInit(THint hint, float heightRatio)
        {
            _hint = hint;
            _hintOffset = AHint.GetOffsetHint(_thisRectTransform, heightRatio);
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
