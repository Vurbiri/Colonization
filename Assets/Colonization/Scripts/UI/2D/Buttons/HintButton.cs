using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class HintButton : AHintButton2D
    {
        [SerializeField] private FileIdAndKey _getText;

        private Unsubscription _unsubscriber;

        public void Init(CanvasHint hint, Action action, bool interactable = true)
        {
            base.Init(hint, 0.5f);

            _onClick.Add(action);
            Interactable = interactable;
            _unsubscriber = Localization.Instance.Subscribe(SetLocalizationText);
        }

        private void SetLocalizationText(Localization localization) => _text = localization.GetText(_getText.id, _getText.key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscriber?.Unsubscribe();
        }
    }
}
