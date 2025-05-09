//Assets\Colonization\Scripts\Characteristics\Buffs\Abstract\ABuff.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuff
    {
        protected readonly Signer<IPerk> _signer;
        protected readonly Effect _base, _current;

        public IPerk Base => _base;
        public IPerk Current => _current;

        public ABuff(Signer<IPerk> subscriber, BuffSettings settings)
        {
            _signer = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, 0);
        }

        protected ABuff(Signer<IPerk> subscriber, BuffSettings settings, int value)
        {
            _signer = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, value);
        }
    }
}
