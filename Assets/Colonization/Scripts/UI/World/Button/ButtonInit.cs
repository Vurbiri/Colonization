using System;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.UI;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
	public class ButtonInit : AWorldHintButton
    {
        [Space]
        [SerializeField] private Image _buttonIcon;
        [Space]
        [SerializeField] private ButtonView _portOneView;
        [SerializeField] private ButtonView _portTwoView;

        private Localization _localization;

        public void Init(ButtonSettings settings, Action action)
        {
            base.Init(settings.hint, action, false);
            _localization = Localization.Instance;
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
            _text = _localization.GetText(Files.Gameplay, view.keyName);

            _thisGameObject.SetActive(true);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_buttonIcon == null)
                _buttonIcon = EUtility.GetComponentInChildren<Image>(this, "Icon");

            if (_portOneView.sprite == null)
                _portOneView.sprite = EUtility.FindAnyAsset<Sprite>("SP_IconPortOne");

            if (_portTwoView.sprite == null)
                _portTwoView.sprite = EUtility.FindAnyAsset<Sprite>("SP_IconPortTwo");

            if (string.IsNullOrEmpty(_portOneView.keyName))
                _portOneView.keyName = "PortOne";

            if (string.IsNullOrEmpty(_portTwoView.keyName))
                _portTwoView.keyName = "PortTwo";
        }
#endif
    }
}
