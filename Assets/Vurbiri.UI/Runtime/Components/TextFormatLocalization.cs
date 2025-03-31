//Assets\Vurbiri.UI\Runtime\Components\TextFormatLocalization.cs
using TMPro;
using UnityEngine;
using Vurbiri.TextLocalization;

namespace Vurbiri.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextFormatLocalization : TextLocalization
    {
        private float _value = 0;

        public void Setup(float value, string key = null)
        {
            _value = value;
            Setup(key);
        }

        protected override void SetText(Localization localization) => _text.text = localization.GetTextFormat(_file, _key, _value);
    }
}
