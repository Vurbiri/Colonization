using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField, Key(Files.Gameplay)] private string _key;

        private ACurrencies _cost;
        private ACurrencies _cash;
        private Unsubscription _unsubscriber;
        private string _caption;

        public void Init(ButtonSettings settings, ACurrencies cost, Action action)
        {
            base.Init(settings, action);
            _cost = cost;
            _cash = settings.player.Resources;
            _unsubscriber = Localization.Instance.Subscribe(SetText);
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

        private void SetText(Localization localization) => _caption = localization.GetText(Files.Gameplay, _key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscriber?.Unsubscribe();
        }
    }
}
