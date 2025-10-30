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

        private ReadOnlyCurrencies _cash;

        public void Init(Action action)
        {
            _cash = GameContainer.Players.Person.Resources;
            base.InternalInit(action, true);
        }

        public void Setup(bool isEnable, bool isUnlock, int edificeId)
        {
            if (!isEnable)
            {
                _thisGameObject.SetActive(false);
                return;
            }

            var view = _edificeView[edificeId];
            var cost = GameContainer.Prices.Edifices[edificeId];

            InteractableAndUnlock(_cash >= cost, isUnlock);
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
                    _edificeView[i].sprite = EUtility.FindAnyAsset<Sprite>($"SP_Icon{EdificeId.Names_Ed[i]}");

                if (string.IsNullOrEmpty(_edificeView[i].keyName))
                    _edificeView[i].keyName = EdificeId.Names_Ed[i];
            }
        }
#endif
    }
}
