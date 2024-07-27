using TMPro;
using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalization : MonoBehaviour
    {
        [SerializeField] protected TextFiles _file;

        public TMP_Text Text { get; protected set; }
        protected string _key;
        protected Localization _localization;

        public void Setup(string key = null)
        {
            _localization = Localization.Instance;
            Text = GetComponent<TMP_Text>();
            _key = string.IsNullOrEmpty(key) ? Text.text : key;

            SetText();
            _localization.EventSwitchLanguage += SetText;
        }

        protected virtual void SetText() => Text.text = _localization.GetText(_file, _key);

        private void OnDestroy()
        {
            if (_localization != null)
                _localization.EventSwitchLanguage -= SetText;
        }
    }
}
