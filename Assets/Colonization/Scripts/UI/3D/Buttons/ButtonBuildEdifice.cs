using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonBuildEdifice : AButtonBuild
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private IdArray<EdificeId, ButtonView> _edificeView;

        private Localization _localization;
        private IReadOnlyList<ACurrencies> _edificePrices;
        private ACurrencies _cash;

        public void Init(ButtonSettings settings, IReadOnlyList<ACurrencies> edificePrices, Action action)
        {
            base.Init(settings, action);
            _localization = Localization.Instance;
            _edificePrices = edificePrices;
            _cash = settings.player.Resources;
        }

        public void Setup(bool isEnable, bool isUnlock, int edificeId)
        {
            if (!isEnable)
            {
                _thisGameObject.SetActive(false);
                return;
            }

            ButtonView view = _edificeView[edificeId];
            ACurrencies cost = _edificePrices[edificeId];

            CombineInteractable(isUnlock, _cash >= cost);
            _buttonIcon.sprite = view.sprite;

            SetTextHint(_localization.GetText(LangFiles.Gameplay, view.keyName), _cash, cost);

            _thisGameObject.SetActive(true);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_buttonIcon == null)
                _buttonIcon = EUtility.GetComponentInChildren<Image>(this, "Icon");

            for (int i = 0; i < EdificeId.Count; i++)
            {
                if (i > 0 && _edificeView[i].sprite == null)
                    _edificeView[i].sprite = EUtility.FindAnyAsset<Sprite>($"SP_Icon{EdificeId.GetName(i)}");

                if (string.IsNullOrEmpty(_edificeView[i].keyName))
                    _edificeView[i].keyName = EdificeId.GetName(i);
            }
        }
#endif
    }
}
