//Assets\Vurbiri\Runtime\Localization\Scripts\Compoments\TextFormatLocalization.cs
using TMPro;
using UnityEngine;
using Vurbiri.Localization;

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

        protected override void SetText(Language localization) => _text.text = localization.GetTextFormat(_file, _key, _value);
    }
}
