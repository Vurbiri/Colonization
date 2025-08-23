namespace Vurbiri.Colonization.UI
{
	public abstract class AValueEffectUI : AEffectUI
    {
        protected readonly string _descKey;
        protected readonly string _value;
        protected readonly string _hexColor;

        protected readonly AEffectUI _advEffect;

        public AValueEffectUI(string key, string value, string hexColor, AEffectUI advEffect)
        {
            _descKey = key;
            _value = value;
            _hexColor = hexColor;
            _advEffect = advEffect;
        }
    }
}
