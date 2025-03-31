//Assets\Vurbiri.UI\Runtime\Components\TextLocalization.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalization : MonoBehaviour
    {
        [SerializeField] protected Files _file;

        protected TMP_Text _text;
        protected string _key;
        protected Unsubscriber _subscribe;

        public TMP_Text Text => _text;

        public void Setup(string key = null)
        {
            _text = GetComponent<TMP_Text>();

            _key = string.IsNullOrEmpty(key) ? _text.text : key;

            _subscribe = Localization.Instance.Subscribe(SetText);
        }

        protected virtual void SetText(Localization localization) => _text.text = localization.GetText(_file, _key);

        private void OnDestroy() => _subscribe.Unsubscribe();
    }
}
