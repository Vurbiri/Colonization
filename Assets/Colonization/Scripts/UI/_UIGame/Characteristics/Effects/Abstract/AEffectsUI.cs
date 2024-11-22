//Assets\Colonization\Scripts\UI\_UIGame\Characteristics\Effects\Abstract\AEffectsUI.cs
using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEffectsUI
	{
        protected readonly string _descKey;
        protected readonly int _value;
        protected readonly string _hexColor;

        public AEffectsUI(string key, int value, string hexColor)
        {
            _descKey = key;
            _value = value;
            _hexColor = hexColor;
        }

        public abstract void GetText(Language language, StringBuilder sb);
    }
}
