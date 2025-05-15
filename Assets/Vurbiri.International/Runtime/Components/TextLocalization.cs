//Assets\Vurbiri.International\Runtime\Components\TextLocalization.cs
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.International.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalization : MonoBehaviour
    {
        [SerializeField] private Files _file;
        [SerializeField] private string _key;
        [Space]
        [SerializeField] private TMP_Text _text;

        private Unsubscriber _subscribe;

        public TMP_Text Text => _text;

        private void Start()
        {
            _subscribe += Localization.Instance.Subscribe(SetText);
        }

        public void Setup(Files file, string key)
        {
            _file = file;
            _key = key;

            _subscribe += Localization.Instance.Subscribe(SetText);
        }

        private void SetText(Localization localization) => _text.text = localization.GetText(_file, _key);

        private void OnDestroy() => _subscribe.Unsubscribe();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();
        }
#endif
    }
}
