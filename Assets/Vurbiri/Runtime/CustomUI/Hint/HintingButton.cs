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

        public void Init(Vector3 localPosition, Color color, UnityEngine.Events.UnityAction action)
        {
            base.Init(localPosition, action, true);
            _button.targetGraphic.color = color;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Init(Vector3 localPosition, UnityEngine.Events.UnityAction action)
        {
            base.Init(localPosition, action, true);
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Setup(bool isEnable)
        {
            if (!isEnable)
            {
                _thisGO.SetActive(false);
                return;
            }

            _thisGO.SetActive(true);
        }

        private void SetText(Language localization) => _text = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
