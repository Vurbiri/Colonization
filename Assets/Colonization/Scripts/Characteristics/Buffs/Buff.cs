//Assets\Colonization\Scripts\Characteristics\Buffs\Buff.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class Buff : IBuff
    {
        private readonly Perk _base, _current;
        private readonly Subscriber<IPerk> _subscriber;

        public int Level => _current.Value / _base.Value;

        public Buff(Subscriber<IPerk> subscriber, int targetAbility, Id<TypeModifierId> typeModifier, int value)
        {
            _subscriber = subscriber;
            _base = new(targetAbility, typeModifier, value);
            _current = new(targetAbility, typeModifier, 0);
        }

        public Buff(Subscriber<IPerk> subscriber, int targetAbility, Id<TypeModifierId> typeModifier, int value, int level)
        {
            _subscriber = subscriber;
            _base = new(targetAbility, typeModifier, value);
            _current = new(targetAbility, typeModifier, value * level);
        }

        public int Apply(Func<IPerk, int> func) => func(_current);

        public void Next()
        {
            _current.Add(_base);
            _subscriber.Invoke(_base);
        }
    }
}
