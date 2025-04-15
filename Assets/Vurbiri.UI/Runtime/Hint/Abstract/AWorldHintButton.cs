//Assets\Vurbiri.UI\Runtime\Hint\Abstract\AWorldHintButton.cs
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
    public abstract class AWorldHintButton : VButton
    {
        private bool _isShowingHint = false;

        protected WorldHint _hint;
        protected Vector3 _offsetHint;
        protected GameObject _thisGameObject;
        protected Transform _thisTransform;
        protected string _text;

        protected virtual void Init(Vector3 localPosition, WorldHint hint, Action action, bool active)
        {
            transform.localPosition = localPosition;

            Init(hint, action, active);
        }

        protected virtual void Init(WorldHint hint, Action action, bool active)
        {
            _hint = hint;

            _thisGameObject = gameObject;
            _thisTransform = transform;

            _onClick.Add(action);

            float offset = GetComponent<RectTransform>().sizeDelta.y * 0.5263f;
            _offsetHint = new(0f, offset, 0f);

            _thisGameObject.SetActive(active);
        }

        sealed public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _thisTransform.localPosition + _offsetHint);
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
