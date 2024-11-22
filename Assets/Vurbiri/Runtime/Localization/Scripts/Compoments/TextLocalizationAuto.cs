//Assets\Vurbiri\Runtime\Localization\Scripts\Compoments\TextLocalizationAuto.cs
using TMPro;
using UnityEngine;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalizationAuto : TextLocalization
    {
        private void Awake() => Setup();
    }
}
