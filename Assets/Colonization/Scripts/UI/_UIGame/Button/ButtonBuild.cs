using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField] private string _key;

        private Unsubscriber<Language> _unsubscriber;
        private string _caption;

        public override void Init()
        {
            base.Init();
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void SetupHint(ACurrencies cash, ACurrencies cost) => SetTextHint(_caption, cash, cost);

        private void SetText(Language localization) => _caption = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
