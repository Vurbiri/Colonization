using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
	public abstract class AHintButton : AVButton
    {
        private AHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _hintOffset;

        protected string _hintText;

        protected void InternalInit(AHint hint, float heightRatio)
        {
            _hint = hint;
            if (_thisRectTransform == null)
                _thisRectTransform = GetComponent<RectTransform>();

            _hintOffset = AHint.GetOffsetHint(_thisRectTransform, heightRatio);
        }

        protected void InternalInit(AHint hint, Action action, float heightRatio)
        {
            InternalInit(hint, heightRatio);
            _onClick.Add(action);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Hide()
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }
    }
}
