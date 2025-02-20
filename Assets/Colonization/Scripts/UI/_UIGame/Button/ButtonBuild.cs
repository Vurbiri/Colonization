//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonBuild.cs
using UnityEngine;
using UnityEngine.Events;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField] protected string _key;

        private ACurrencies _cost;
        private ACurrencies _cash;
        private IUnsubscriber _unsubscriber;
        private string _caption;

        public virtual void Init(Vector3 localPosition, ButtonSettings settings, ACurrencies cost, UnityAction action)
        {
            base.Init(localPosition, settings, action);
            _cost = cost;
            _cash = settings.player.Resources;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
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

        private void SetText(Language localization) => _caption = localization.GetText(Files.Gameplay, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
