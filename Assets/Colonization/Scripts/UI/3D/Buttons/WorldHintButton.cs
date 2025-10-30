using System;
using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class WorldHintButton : AHintButton3D
    {
        [SerializeField] private FileIdAndKey _getText;

        private Subscription _subscription;

        public void Init(Action action)
        {
            base.InternalInit(action, true);

            _subscription = Localization.Instance.Subscribe(SetLocalizationText);
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
            _subscription?.Dispose();
        }
    }
}
