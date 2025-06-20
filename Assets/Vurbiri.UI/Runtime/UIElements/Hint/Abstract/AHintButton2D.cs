using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AHintButton2D : AVButton
    {
        private CanvasHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _offsetHint;

        protected Transform _thisTransform;
        protected string _text;

        protected void Init(CanvasHint hint, float ratioHeight)
        {
            _hint = hint;
            _thisTransform = transform;

            RectTransform thisRectTransform = (RectTransform)_thisTransform;
            Vector2 pivot = thisRectTransform.pivot;
            Vector2 size = thisRectTransform.rect.size;

            _offsetHint = new(size.x * (0.5f - pivot.x), size.y * (0.5f - pivot.y + ratioHeight), 0f);
        }

        sealed public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _thisTransform.position, _offsetHint);
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
