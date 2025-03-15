//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonBuild.cs
using UnityEngine;
using UnityEngine.Events;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField] private string _key;

        private ACurrencies _cost;
        private ACurrencies _cash;
        private Unsubscriber _unsubscriber;
        private string _caption;

        public void Init(Vector3 localPosition, ButtonSettings settings, ACurrencies cost, UnityAction action)
        {
            base.Init(localPosition, settings, action);
            _cost = cost;
            _cash = settings.player.Resources;
            _unsubscriber = SceneServices.Get<Localization>().Subscribe(SetText);
        }

        public void Setup(bool isEnable)
        {
            if(!isEnable)
            {
                _thisGO.SetActive(false);
                return;
            }
            
            _button.Interactable = _cash >= _cost;

            SetTextHint(_caption, _cash, _cost);

            _thisGO.SetActive(true);
        }

        private void SetText(Localization localization) => _caption = localization.GetText(Files.Gameplay, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
