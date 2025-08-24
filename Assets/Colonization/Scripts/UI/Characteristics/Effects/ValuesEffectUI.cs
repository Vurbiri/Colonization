using System.Text;
using Vurbiri.International;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
    sealed public class ValuesEffectUI : AValueEffectUI
    {
        private readonly string[] _values;

        [Impl(256)] public ValuesEffectUI(string descKey, string main, string adv, string hexColor, AEffectUI advEffect) : base(descKey, hexColor, advEffect)
        {
            _values = new string[] { main, adv };
        }
        [Impl(256)] public ValuesEffectUI(string descKey, string main, string advA, string advB, string hexColor, AEffectUI advEffect) : base(descKey, hexColor, advEffect)
        {
            _values = new string[] { main, advA, advB };
        }

        [Impl(256)] public ValuesEffectUI(string descKey, string main, int adv, string hexColor, AEffectUI advEffect) : this(descKey, main, adv.ToString(), hexColor, advEffect) { }
        [Impl(256)] public ValuesEffectUI(string descKey, string main, int adv, string hexColor) : this(descKey, main, adv.ToString(), hexColor, EffectUI.Empty) { }

        [Impl(256)] public ValuesEffectUI(string descKey, string main, int advA, int advB, string hexColor, AEffectUI advEffect)
                     : this(descKey, main, advA.ToString(), advB.ToString(), hexColor, advEffect) { }
        [Impl(256)] public ValuesEffectUI(string descKey, string main, int advA, int advB, string hexColor) 
                     : this(descKey, main, advA.ToString(), advB.ToString(), hexColor, EffectUI.Empty) { }

        
        public override void GetText(Localization language, StringBuilder sb)
        {
            sb.Append(_hexColor);
            sb.AppendLine(language.GetFormatText(CONST_UI_LNG_KEYS.FILE, _descKey, _values));
            _advEffect.GetText(language, sb);
        }
    }
}
