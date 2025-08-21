namespace Vurbiri.Colonization.UI
{
	public abstract class AMainEffectsUI : AEffectsUI
    {
        protected readonly string _descKey;
        protected readonly string _main;
        protected readonly string _hexColor;

        protected readonly AEffectsUI _advEffect;

        public AMainEffectsUI(string key, string main, string hexColor, AEffectsUI advEffect)
        {
            _descKey = key;
            _main = main;
            _hexColor = hexColor;
            _advEffect = advEffect;
        }
    }
}
