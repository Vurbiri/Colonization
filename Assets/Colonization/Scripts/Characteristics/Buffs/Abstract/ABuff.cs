using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuff
    {
        protected readonly Subscription<IPerk> _subscriber;
        protected readonly Effect _base, _current;

        public IPerk Base => _base;
        public IPerk Current => _current;

        public ABuff(Subscription<IPerk> subscriber, BuffSettings settings)
        {
            _subscriber = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, 0);
        }

        protected ABuff(Subscription<IPerk> subscriber, BuffSettings settings, int value)
        {
            _subscriber = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, value);
        }
    }
}
