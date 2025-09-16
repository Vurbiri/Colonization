using System;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class ButtonInit : AHintButton3D
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private ButtonView _portOneView;
        [SerializeField] private ButtonView _portTwoView;

        public void Init(Action action)
        {
            base.InternalInit(GameContainer.UI.WorldHint, action, false);
        }

        public void Setup(bool isEnable, int edificeId)
        {
            if (!isEnable || (edificeId != EdificeId.PortOne & edificeId != EdificeId.PortTwo))
            {
                _thisGameObject.SetActive(false);
                return;
            }

            ButtonView view = edificeId == EdificeId.PortOne ? _portOneView : _portTwoView;

            _buttonIcon.sprite = view.sprite;
            _hintText = Localization.Instance.GetText(LangFiles.Gameplay, view.keyName);

            _thisGameObject.SetActive(true);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetChildren(ref _buttonIcon, "Icon");

            EUtility.SetAsset(ref _portOneView.sprite, "SP_IconPortOne");
            EUtility.SetAsset(ref _portTwoView.sprite, "SP_IconPortTwo");

            if (string.IsNullOrEmpty(_portOneView.keyName))
                _portOneView.keyName = "PortOne";

            if (string.IsNullOrEmpty(_portTwoView.keyName))
                _portTwoView.keyName = "PortTwo";
        }
#endif
    }
}
