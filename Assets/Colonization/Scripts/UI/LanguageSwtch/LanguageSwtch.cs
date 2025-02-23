//Assets\Colonization\Scripts\UI\LanguageSwtch\LanguageSwtch.cs
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization.UI
{
    public class LanguageSwitch : ToggleGroup
    {
        [SerializeField] private LanguageItem _langPrefab;
        [Space]
        [SerializeField] private bool _isSave = false;

        protected override void Awake()
        {
            base.Awake();

            allowSwitchOff = false;
            var languages = SceneServices.Get<Localization>().Languages;
            foreach (var item in languages)
                Instantiate(_langPrefab, transform).Setup(item, this, _isSave);
        }
    }
}
