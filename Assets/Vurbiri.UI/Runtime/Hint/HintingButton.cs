//Assets\Vurbiri.UI\Runtime\Hint\HintingButton.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.UI
{
    public class HintingButton : AHintingButton
    {
        [SerializeField] protected Files _file;
        [SerializeField] private string _key;

        private Unsubscriber _unsubscriber;

        public void Init(Vector3 localPosition, HintGlobal hint, Color color, Action action)
        {
            base.Init(localPosition, hint, action, true);
            _button.targetGraphic.color = color;
            _unsubscriber = SceneContainer.Get<Localization>().Subscribe(SetText);
        }

        public void Init(HintGlobal hint, Action action)
        {
            base.Init(hint, action, true);
            _unsubscriber = SceneContainer.Get<Localization>().Subscribe(SetText);
        }

        public void Setup(bool isEnable, bool interactable = true)
        {
            _button.interactable = interactable;
            _thisGO.SetActive(isEnable);
        }

        private void SetText(Localization localization) => _text = localization.GetText(_file, _key);

        private void OnDestroy()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}
