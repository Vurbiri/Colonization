using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class ABuff
    {
        protected readonly VAction<IPerk> _subscriber;
        protected readonly Effect _base, _current;

        public Effect Base => _base;
        public Effect Current => _current;

        public ABuff(VAction<IPerk> subscriber, BuffSettings settings)
        {
            _subscriber = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, 0);
        }

        protected ABuff(VAction<IPerk> subscriber, BuffSettings settings, int value)
        {
            _subscriber = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, value);
        }
    }
}
