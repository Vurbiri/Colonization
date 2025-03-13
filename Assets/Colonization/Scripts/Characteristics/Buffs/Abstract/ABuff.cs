//Assets\Colonization\Scripts\Characteristics\Buffs\Abstract\ABuff.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuff<T> : IBuff where T : BuffSettings
    {
        protected readonly Subscriber<IPerk> _subscriber;
        protected readonly Perk _base, _current;

        public ABuff(Subscriber<IPerk> subscriber, T settings)
        {
            _subscriber = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, 0);
        }

        protected ABuff(Subscriber<IPerk> subscriber, BuffSettings settings, int value)
        {
            _subscriber = subscriber;
            _base = new(settings.targetAbility, settings.typeModifier, settings.value);
            _current = new(settings.targetAbility, settings.typeModifier, value);
        }

        public int Apply(Func<IPerk, int> func) => func(_current);
    }
}
