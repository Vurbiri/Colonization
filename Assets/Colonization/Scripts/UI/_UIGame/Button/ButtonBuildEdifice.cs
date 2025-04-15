//Assets\Colonization\Scripts\UI\_UIGame\Button\ButtonBuildEdifice.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    //[RequireComponent(typeof(VButton))]
    sealed public class ButtonBuildEdifice : AButtonBuild
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private IdArray<EdificeId, ButtonView> _edificeView;

        private Localization _localization;
        private IReadOnlyList<ACurrencies> _edificePrices;
        private ACurrencies _cash;

        public void Init(Vector3 localPosition, ButtonSettings settings, IReadOnlyList<ACurrencies> edificePrices, Action action)
        {
            base.Init(localPosition, settings, action);
            _localization = SceneContainer.Get<Localization>();
            _edificePrices = edificePrices;
            _cash = settings.player.Resources;
        }

        public void Setup(bool isEnable, int edificeId)
        {
            if (!isEnable)
            {
                _thisGameObject.SetActive(false);
                return;
            }

            ButtonView view = _edificeView[edificeId];
            ACurrencies cost = _edificePrices[edificeId];

            interactable = _cash >= cost;
            _buttonIcon.sprite = view.sprite;

            SetTextHint(_localization.GetText(Files.Gameplay, view.keyHint), _cash, cost);

            _thisGameObject.SetActive(true);
        }

        #region Nested: ButtonView
        //*******************************************************
        [System.Serializable]
        private class ButtonView
        {
            public Sprite sprite;
            public string keyHint;
        }
        #endregion


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base .OnValidate();

            for (int i = 0; i < EdificeId.Count; i++)
            {
                if(string.IsNullOrEmpty(_edificeView[i].keyHint))
                _edificeView[i].keyHint = EdificeId.GetName(i);
            }
        }
#endif
    }
}
