//Assets\Vurbiri.UI\Runtime\Hint\HintingButton.cs
using UnityEngine;
using UnityEngine.Events;
using Vurbiri.Localization;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    public class HintingButton : AHintingButton
    {
        [SerializeField] protected Files _file;
        [SerializeField] private string _key;

        private IUnsubscriber _unsubscriber;

        public void Init(Vector3 localPosition, HintGlobal hint, Color color, UnityAction action)
        {
            base.Init(localPosition, hint, action, true);
            _button.targetGraphic.color = color;
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Init(HintGlobal hint, UnityAction action)
        {
            base.Init(hint, action, true);
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
        }

        public void Setup(bool isEnable, bool interactable = true)
        {
            _button.Interactable = interactable;
            _thisGO.SetActive(isEnable);
        }

        private void SetText(Language localization) => _text = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
