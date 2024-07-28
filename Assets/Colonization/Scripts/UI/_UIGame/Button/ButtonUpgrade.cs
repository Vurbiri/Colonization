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
        [Space]
        [SerializeField] private EnumArray<EdificeType, Sprite> _edificeSprites;

        public CmButton Button => _button;

        private CmButton _button;

        public override void Initialize()
        {
            base.Initialize();
            _button = _thisSelectable as CmButton;
        }

        public void SetupHint(EdificeType type)
        {
            _buttonIcon.sprite = _edificeSprites[type];

            _keyType = type.ToString();
            _text = GetTextFormat();
        }

        public void AddListener(UnityEngine.Events.UnityAction action) => _button.onClick.AddListener(action);

        protected override void SetText() => _text = GetTextFormat();

        private string GetTextFormat() => string.Format(_format, _localization.GetText(_file, _keyType));
    }
}
