using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonHiring : AButtonBuild
    {
        [Space]
        [SerializeField] private Image _buttonIcon;

        private int _id;
        private string _key;
        private string _caption;
        private Unsubscriber<Language> _unsubscriber;
        private ACurrencies _cost;

        public int Id => _id;

        internal ButtonHiring Init(int id, HintGlobal hint, ACurrencies cost,ButtonView view)
        {
            base.Init();
            _id = id;
            _hint = hint;
            _cost = cost;
            _key = view.keyHint;
            _buttonIcon.sprite = view.sprite;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);

            return this;
        }

        public void SetupHint(ACurrencies cash) => SetTextHint(_caption, cash, _cost);

        private void SetText(Language localization) => _caption = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
