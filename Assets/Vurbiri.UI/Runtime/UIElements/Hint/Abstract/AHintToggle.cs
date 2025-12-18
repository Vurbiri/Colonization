using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public abstract class AHintToggle<TToggle> : VToggleGraphic<TToggle> where TToggle : AHintToggle<TToggle>
    {
        private Id<HintId> _hint;
        private bool _isShowingHint = false;
        private Vector3 _hintOffset;

        protected string _hintText;

        [Impl(256)]
        protected void InternalInit(Id<HintId> hint, bool value, float heightRatio)
        {
            _hint = hint;
            SetValue(value, false);

            _hintOffset = _thisRectTransform.GetOffsetHint(heightRatio);
        }

        [Impl(256)]
        protected void InternalInit(Id<HintId> hint, bool value, Action<bool> action, float heightRatio)
        {
            InternalInit(hint, value, heightRatio);
            _onValueChanged.Add(action);
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
