using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.UI
{
    sealed public class WorldHintButton : AWorldHintButton
    {
        [SerializeField] private FileIdAndKey _getText;

        private Unsubscription _unsubscriber;

        public void Init(WorldHint hint, Action action)
        {
            base.Init(hint, action, true);

            _unsubscriber = Localization.Instance.Subscribe(SetText);
        }

        public void Setup(bool isEnable, bool interactable = true)
        {
            this.interactable = interactable;
            _thisGameObject.SetActive(isEnable);
        }

        private void SetText(Localization localization) => _text = localization.GetText(_getText.id, _getText.key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscriber?.Unsubscribe();
        }
    }
}
