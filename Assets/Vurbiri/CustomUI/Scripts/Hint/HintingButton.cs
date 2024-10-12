using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public class HintingButton : AHinting
    {
        [Space]
        [SerializeField] private string _key;

        private Unsubscriber<Language> _unsubscriber;

        private void Start() => Init();

        public override void Init()
        {
            _thisTransform = transform;
            _unsubscriber = Language.Subscribing(SetText);
        }

        private void SetText(Language localization) => _text = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
