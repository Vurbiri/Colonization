using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AHintElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private AHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _offsetHint;

        protected Transform _thisTransform;
        protected string _hintText;

        protected void Init(AHint hint, float ratioHeight)
        {
            _hint = hint;
            _thisTransform = transform;

            RectTransform thisRectTransform = (RectTransform)_thisTransform;
            Vector2 pivot = thisRectTransform.pivot;
            Vector2 size = thisRectTransform.rect.size;

            _offsetHint = new(size.x * (0.5f - pivot.x), size.y * (0.5f - pivot.y + ratioHeight), 0f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_hintText, _thisTransform.position, _offsetHint);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        protected virtual void OnDisable()
        {
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
