using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CmButton))]
    public class ButtonUpgrade : AHinting
    {
        [Space]
        [SerializeField] private string _keyType = "Shrine";
        [SerializeField, Multiline] private string _format = "<align=\"center\"><b>{0}</b>\r\n";
        [Space]
        [SerializeField] private Image _buttonIcon;


        public bool Interactable { set => _button.interactable = value; }

        private CmButton _button;
        private Graphic _buttonGraphic;

        public override void Initialize()
        {
            base.Initialize();

            _button = _thisSelectable as CmButton;
            _buttonGraphic = _button.targetGraphic;
        }

        protected override void SetText() => _text = GetTextFormat();

        public bool Setup(EdificeType type, Sprite sprite, Color color, UnityEngine.Events.UnityAction action)
        {
            if (type == EdificeType.None)
            {
                gameObject.SetActive(false);
                return false;
            }

            gameObject.SetActive(true);
            _buttonGraphic.color = color;
            _buttonIcon.sprite = sprite;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(action);

            _keyType = type.ToString();
            _text = GetTextFormat();

            return true;
        }

        private string GetTextFormat() => string.Format(_format, _localization.GetText(_file, _keyType));
    }
}
