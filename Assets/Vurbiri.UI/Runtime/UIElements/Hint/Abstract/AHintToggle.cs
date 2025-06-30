using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AHintToggle<TToggle> : VToggle<TToggle> where TToggle : AHintToggle<TToggle>
    {
        private AHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _offsetHint;

        protected string _text;

        protected void Init(AHint hint, float ratioHeight)
        {
            _hint = hint;
            if (_rectTransform == null)
                _rectTransform = (RectTransform)transform;

            Vector2 pivot = _rectTransform.pivot;
            Vector2 size = _rectTransform.rect.size;

            _offsetHint = new(size.x * (0.5f - pivot.x), size.y * (0.5f - pivot.y + ratioHeight), 0f);
        }

        sealed public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _rectTransform.position, _offsetHint);
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
