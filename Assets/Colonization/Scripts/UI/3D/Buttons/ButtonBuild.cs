using System;
using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField, Key(LangFiles.Gameplay)] private string _key;

        private ReadOnlyMainCurrencies _cost;
        private ReadOnlyCurrencies _cash;
        private Subscription _subscription;
        private string _caption;

        public void Init(ReadOnlyMainCurrencies cost, Action action)
        {
            base.InternalInit(GameContainer.UI.WorldHint, action, true);
            _cost = cost;
            _cash = GameContainer.Players.Person.Resources;
            _subscription = Localization.Instance.Subscribe(SetText);
        }

        public void Setup(bool isEnable)
        {
            if(!isEnable)
            {
                _thisGameObject.SetActive(false);
                return;
            }
            
            Interactable = _cash >= _cost;
            SetTextHint(_caption, _cash, _cost);

            _thisGameObject.SetActive(true);
        }

        public void Setup(bool isEnable, bool isUnlock)
        {
            if (!isEnable)
            {
                _thisGameObject.SetActive(false);
                return;
            }

            CombineInteractable(isUnlock, _cash >= _cost);
            SetTextHint(_caption, _cash, _cost);

            _thisGameObject.SetActive(true);
        }

        private void SetText(Localization localization) => _caption = localization.GetText(LangFiles.Gameplay, _key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _subscription?.Dispose();
        }
    }
}
