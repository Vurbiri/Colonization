using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField] private string _key;

        private ACurrencies _cost;
        private Unsubscriber<Language> _unsubscriber;
        private string _caption;

        public void Init(ACurrencies cost)
        {
            base.Init();
            _cost = cost;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void SetupHint(ACurrencies cash) => SetTextHint(_caption, cash, _cost);

        private void SetText(Language localization) => _caption = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
