using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Vurbiri.Localization;

namespace Vurbiri.UI
{
    public abstract class AHintingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected Files _file;
        [SerializeField] protected Vector3 _offset;

        private bool _isShowingHint = false;

        protected HintGlobal _hint;
        protected GameObject _thisGO;
        protected Transform _thisTransform;
        protected CmButton _button;
        protected string _text;

        public bool IsShowingHint => _isShowingHint;

        protected virtual void Init(Vector3 localPosition, HintGlobal hint, UnityAction action, bool active)
        {
            transform.localPosition = localPosition;

            Init(hint, action, active);
        }

        protected virtual void Init(HintGlobal hint, UnityAction action, bool active)
        {
            _hint = hint;

            _thisGO = gameObject;
            _thisTransform = transform;

            _button = GetComponent<CmButton>();
            _button.onClick.AddListener(action);

            _thisGO.SetActive(active);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _thisTransform.localPosition + _offset);
        }
        public void OnPointerExit(PointerEventData eventData) => HideHint();

        protected void OnDisable() => HideHint();

        private void HideHint()
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }
    }
}
