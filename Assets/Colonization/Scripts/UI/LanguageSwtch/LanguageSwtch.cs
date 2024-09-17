using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Localization;

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
            foreach (var item in Language.Instance.Languages)
                Instantiate(_langPrefab, transform).Setup(item, this, _isSave);
        }
    }
}
