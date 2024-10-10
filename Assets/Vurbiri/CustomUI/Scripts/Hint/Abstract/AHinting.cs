using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public abstract class AHinting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private HintGlobal _hint;
        [SerializeField] protected Files _file;
        [SerializeField] protected Vector3 _offset;

        public bool IsShowingHint => _isShowingHint;

        private bool _isShowingHint = false;
        protected Transform _thisTransform;
        protected string _text;
        protected Unsubscriber<Language> _unsubscriber;

        public virtual void Init()
        {
            _thisTransform = transform;
            _unsubscriber = Language.Subscribing(SetText);
        }

        protected abstract void SetText(Language localization);

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

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_hint == null)
                _hint = FindAnyObjectByType<HintGlobal>();
        }
#endif
    }
}
