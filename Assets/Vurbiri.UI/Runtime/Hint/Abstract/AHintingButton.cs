//Assets\Vurbiri.UI\Runtime\Hint\Abstract\AHintingButton.cs
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vurbiri.UI
{
    public abstract class AHintingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _isShowingHint = false;

        protected HintGlobal _hint;
        protected Vector3 _offsetHint;
        protected GameObject _thisGO;
        protected Transform _thisTransform;
        protected VButton _button;
        protected string _text;

        protected virtual void Init(Vector3 localPosition, HintGlobal hint, Action action, bool active)
        {
            transform.localPosition = localPosition;

            Init(hint, action, active);
        }

        protected virtual void Init(HintGlobal hint, Action action, bool active)
        {
            _hint = hint;

            _thisGO = gameObject;
            _thisTransform = transform;

            _button = GetComponent<VButton>();
            _button.AddListener(action);

            float offset = GetComponent<RectTransform>().sizeDelta.y / 1.9f;
            _offsetHint = new( 0f, offset, 0f);

            _thisGO.SetActive(active);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _thisTransform.localPosition + _offsetHint);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }

        protected virtual void OnDisable()
        {
            OnPointerExit(null);
        }
    }
}
