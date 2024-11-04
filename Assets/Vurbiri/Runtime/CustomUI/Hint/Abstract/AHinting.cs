using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Vurbiri.Localization;

namespace Vurbiri.UI
{
    public abstract class AHinting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected HintGlobal _hint;
        [SerializeField] protected Files _file;
        [SerializeField] protected Vector3 _offset;

        private bool _isShowingHint = false;
       
        protected GameObject _thisGO;
        protected Transform _thisTransform;
        protected CmButton _button;
        protected string _text;

        public bool IsShowingHint => _isShowingHint;

        protected virtual void Init(Vector3 localPosition, UnityAction action, bool active)
        {
            _thisGO = gameObject;
            _thisTransform = transform;

            _thisTransform.localPosition = localPosition;

            _button = GetComponent<CmButton>();
            _button.onClick.AddListener(action);
            
            _thisGO.SetActive(active);
        }

        protected virtual void Init(UnityAction action, bool active)
        {
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

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_hint == null)
                _hint = FindAnyObjectByType<HintGlobal>();
        }
#endif
    }
}
