using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
    public abstract class AHintButton3D : AVButton
    {
        private WorldHint _hint;
        private bool _isShowingHint = false;
        private Vector3 _offsetHint;

        protected GameObject _thisGameObject;
        protected string _text;

        protected virtual void Init(WorldHint hint, Action action, bool active, float ratioHeight = 0.5263f)
        {
            _hint = hint;
            _thisGameObject = gameObject;
            if (_rectTransform == null)
                _rectTransform = (RectTransform)transform;

            Vector2 pivot = _rectTransform.pivot;
            Vector2 size = _rectTransform.rect.size;

            _offsetHint = new(size.x * (0.5f - pivot.x), size.y * (0.5f - pivot.y + ratioHeight), 0f);

            _onClick.Add(action);
            _thisGameObject.SetActive(active);
        }

        sealed public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _rectTransform.localPosition, _offsetHint);
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
