using System;
using UnityEngine;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class WorldHintButton : AHintButton3D
    {
        [SerializeField] private FileIdAndKey _getText;
        [SerializeField] private bool _extract;

        public void Init(Action action)
        {
            base.InternalInit(action, true);
            Localization.Subscribe(SetLocalizationText);
        }

        public void Setup(bool isEnable, bool interactable = true)
        {
            this.interactable = interactable;
            _thisGameObject.SetActive(isEnable);
        }

        private void SetLocalizationText(Localization localization)
        {
            _hintText = localization.GetText(_getText, _extract);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.Unsubscribe(SetLocalizationText);
        }
    }
}
