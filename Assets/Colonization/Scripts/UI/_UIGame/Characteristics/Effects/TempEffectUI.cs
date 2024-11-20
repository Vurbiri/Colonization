using System.Text;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class TempEffectUI : AEffectsUI
    {
        private readonly int _duration;
        
        public TempEffectUI(string descKey, int value, int duration, string hexColor) : base(descKey, value, hexColor)
        {
            _duration = duration;
        }

        public override void GetText(Language language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetTextFormat(Files.Gameplay, _descKey, _value, _duration));
        }
    }
}
