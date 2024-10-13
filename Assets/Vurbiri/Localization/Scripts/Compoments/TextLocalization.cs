using TMPro;
using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalization : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected Files _file;

        protected string _key;
        protected Unsubscriber<Language> _subscribe;

        public TMP_Text Text => _text;

        public void Setup(string key = null)
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();

            _key = string.IsNullOrEmpty(key) ? _text.text : key;

            _subscribe = SceneServices.Get<Language>().Subscribe(SetText);
        }

        protected virtual void SetText(Language localization) => _text.text = localization.GetText(_file, _key);

        private void OnDestroy() => _subscribe.Unsubscribe();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_text == null)
                _text = GetComponent<TMP_Text>();
        }
#endif
    }
}
