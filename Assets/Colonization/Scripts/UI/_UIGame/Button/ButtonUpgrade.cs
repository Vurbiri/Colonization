using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;
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
        //[SerializeField] private EnumArray<EdificeType, Sprite> _edificeSprites;

        private CmButton _button;
        private Language _localization;
        
        public CmButton Button => _button;

        public override void Initialize()
        {
            base.Initialize();
            _button = _thisSelectable as CmButton;
            _localization = Language.Instance;
        }

        public void SetupHint(EdificeId type)
        {
            //_buttonIcon.sprite = _edificeSprites[type];
            Debug.Log("!!!!!!!!!!!!!!!!!!!");

            _keyType = type.ToString();
            _text = GetTextFormat(_localization);
        }

        public void AddListener(UnityEngine.Events.UnityAction action) => _button.onClick.AddListener(action);

        protected override void SetText(Language localization) => _text = GetTextFormat(localization);

        private string GetTextFormat(Language localization) => string.Format(_format, localization.GetText(_file, _keyType));
    }
}
