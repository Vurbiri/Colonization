using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class WorldHintButton : AHintButton3D
    {
        [SerializeField] private FileIdAndKey _getText;

        private Unsubscription _unsubscriber;

        public void Init(Action action)
        {
            base.InternalInit(GameContainer.UI.WorldHint, action, true);

            _unsubscriber = Localization.Instance.Subscribe(SetLocalizationText);
        }

        public void Setup(bool isEnable, bool interactable = true)
        {
            this.interactable = interactable;
            _thisGameObject.SetActive(isEnable);
        }

        private void SetLocalizationText(Localization localization) => _hintText = localization.GetText(_getText.id, _getText.key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscriber?.Unsubscribe();
        }
    }
}
