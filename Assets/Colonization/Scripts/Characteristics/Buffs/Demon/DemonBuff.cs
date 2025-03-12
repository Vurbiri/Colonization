//Assets\Colonization\Scripts\Characteristics\Buffs\Demon\DemonBuff.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class DemonBuff : IDemonBuff
    {
        private readonly Perk _base, _current;
        private readonly Subscriber<IPerk> _subscriber;
        private readonly int _levelUP;

        public DemonBuff(Subscriber<IPerk> subscriber, int targetAbility, Id<TypeModifierId> typeModifier, int value, int levelUP)
        {
            _subscriber = subscriber;
            _base = new(targetAbility, typeModifier, value);
            _current = new(targetAbility, typeModifier, 0);
            _levelUP = levelUP;
        }

        public static IDemonBuff Create(Subscriber<IPerk> subscriber, int targetAbility, Id<TypeModifierId> typeModifier, int value, int levelUP)
        {
            if (typeModifier.Value < 0 | value <= 0)
                return new DemonBuffEmpty();

            return new DemonBuff(subscriber, targetAbility, typeModifier, value, levelUP);
        }

        public int Apply(Func<IPerk, int> func) => func(_current);

        public void Next(int level)
        {
            if (level % _levelUP != 0)
                return;

            _current.Add(_base);
            _subscriber.Invoke(_base);
        }

        #region Nested: DemonBuffEmpty
        //***********************************
        private class DemonBuffEmpty : IDemonBuff
        {
            public DemonBuffEmpty() { }

            public int Apply(Func<IPerk, int> func) => 0;

            public void Next(int level) { }
        }
        #endregion
    }
}
