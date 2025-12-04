using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.International;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
    public class LanguageItem : VToggleGraphic<LanguageItem>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;

        public SystemLanguage Id { [Impl(256)] get; [Impl(256)] private set; }

        public bool Init(SystemLanguage currentId, LanguageType languageType)
        {
            _icon.sprite = languageType.GetSprite();
            _name.text = languageType.Name;
            _icon = null; _name = null;

            Id = languageType.Id;

            _isOn = currentId == Id;
            UpdateVisualInstant();

#if UNITY_EDITOR
            gameObject.name = languageType.Folder;
#endif
            return _isOn;
        }
    }
}
