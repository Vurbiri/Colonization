//Assets\Colonization\Scripts\UI\_UIGame\Button\WorldHintButton.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.UI
{
    sealed public class WorldHintButton : AWorldHintButton
    {
        [SerializeField] private Files _file;
        [SerializeField] private string _key;
        [Space]
        [SerializeField] private int _indexApplyColor;

        private Unsubscriber _unsubscriber;

        public void Init(Vector3 localPosition, WorldHint hint, Color color, Action action)
        {
            base.Init(localPosition, hint, action, true);

            _unsubscriber = Localization.Instance.Subscribe(SetText);

            _targetGraphics[_indexApplyColor].SetGraphicColor(color);
        }

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

        private void SetText(Localization localization) => _text = localization.GetText(_file, _key);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscriber?.Unsubscribe();
        }
    }
}
