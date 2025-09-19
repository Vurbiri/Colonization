using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AHintElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private AHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _hintOffset;

        protected RectTransform _thisRectTransform;
        protected string _hintText;

        protected void InternalInit(AHint hint, float heightRatio = 0.5263f)
        {
            _hint = hint;
            _thisRectTransform = (RectTransform)transform;
            _hintOffset = AHint.GetOffsetHint(_thisRectTransform, heightRatio);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_hintText, _thisRectTransform.position, _hintOffset);
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
