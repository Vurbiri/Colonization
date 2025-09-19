using System;
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

        private ReadOnlyIdArray<EdificeId, CurrenciesLite> _edificePrices;
        private ACurrencies _cash;

        public void Init(ReadOnlyIdArray<EdificeId, CurrenciesLite> edificePrices, Action action)
        {
            base.InternalInit(GameContainer.UI.WorldHint, action, true);
            _edificePrices = edificePrices;
            _cash = GameContainer.Players.Person.Resources;
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

            SetTextHint(Localization.Instance.GetText(LangFiles.Gameplay, view.keyName), _cash, cost);

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
                    _edificeView[i].sprite = EUtility.FindAnyAsset<Sprite>($"SP_Icon{EdificeId.GetName_Ed(i)}");

                if (string.IsNullOrEmpty(_edificeView[i].keyName))
                    _edificeView[i].keyName = EdificeId.GetName_Ed(i);
            }
        }
#endif
    }
}
