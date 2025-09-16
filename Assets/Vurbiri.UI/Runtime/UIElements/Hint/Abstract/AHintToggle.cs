using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AHintToggle<TToggle> : VToggleGraphic<TToggle> where TToggle : AHintToggle<TToggle>
    {
        private AHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _hintOffset;

        protected string _hintText;

        protected void Init(AHint hint, float heightRatio)
        {
            _hint = hint;
            if (_thisRectTransform == null)
                _thisRectTransform = (RectTransform)transform;

            _hintOffset = AHint.GetOffsetHint(_thisRectTransform, heightRatio);
        }

        sealed public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_hintText, _thisRectTransform.position, _hintOffset);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Hide()
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }
    }
}
