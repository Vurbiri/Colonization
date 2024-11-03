using UnityEngine;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    public class ButtonBuild : AButtonBuild
    {
        [Space]
        [SerializeField] protected string _key;

        
        private ACurrencies _cost;
        private Unsubscriber<Language> _unsubscriber;
        private string _caption;

        public virtual void Init(Vector3 localPosition, Color color, ACurrencies cost, UnityEngine.Events.UnityAction action)
        {
            base.Init(localPosition, color, action);
            _cost = cost;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Setup(bool isEnable, ACurrencies cash)
        {
            if(!isEnable)
            {
                _thisGO.SetActive(false);
                return;
            }
            
            _button.Interactable = cash >= _cost;

            SetTextHint(_caption, cash, _cost);

            _thisGO.SetActive(true);
        }

        private void SetText(Language localization) => _caption = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
