using System.Text;
using Vurbiri.International;

namespace Vurbiri.Colonization.UI
{
    public abstract class AEffectsUI
	{
       
        protected readonly string _descKey;
        protected readonly string _value;
        protected readonly string _hexColor;

        public AEffectsUI(string key, string value, string hexColor)
        {
            _descKey = key;
            _value = value;
            _hexColor = hexColor;
        }

        public abstract void GetText(Localization language, StringBuilder sb);
    }
}
