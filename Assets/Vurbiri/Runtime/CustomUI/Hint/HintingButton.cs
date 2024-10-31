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

        public void Init(Vector3 localPosition, UnityEngine.Events.UnityAction action)
        {
            base.Init(localPosition, action);
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Setup(bool isEnable, Color color)
        {
            if (!isEnable)
            {
                _thisGO.SetActive(false);
                return;
            }

            _targetGraphic.color = color;
            _thisGO.SetActive(true);
        }

        private void SetText(Language localization) => _text = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
