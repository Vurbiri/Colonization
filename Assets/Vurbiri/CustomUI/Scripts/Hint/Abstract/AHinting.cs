using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(Selectable))]
    public abstract class AHinting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, FindObject] private HintGlobal _hint;
        [SerializeField] protected TextFiles _file;

        public bool IsShowingHint => _isShowingHint;

        private bool _isShowingHint = false;
        protected Selectable _thisSelectable;
        protected string _text;
        protected Localization _localization;

        public virtual void Initialize()
        {
            _thisSelectable = GetComponent<Selectable>();
            _localization = Localization.Instance;
            if (_hint == null)
                _hint = FindAnyObjectByType<HintGlobal>();

            SetText();
            _localization.EventSwitchLanguage += SetText;
        }

        protected abstract void SetText();

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text);
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
            if (_localization != null)
                _localization.EventSwitchLanguage -= SetText;
        }
    }
}
