//Assets\Vurbiri.UI\Runtime\Localization\TextLocalization.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalization : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _text;
        [SerializeField] protected Files _file;

        protected string _key;
        protected Unsubscriber _subscribe;

        public TMP_Text Text => _text;

        public void Setup(string key = null)
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();

            _key = string.IsNullOrEmpty(key) ? _text.text : key;

            _subscribe = Localization.Instance.Subscribe(SetText);
        }

        protected virtual void SetText(Localization localization) => _text.text = localization.GetText(_file, _key);

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
