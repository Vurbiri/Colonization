using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public abstract class ABuffs<T> : IReactive<IPerk> where T : ABuff
    {
        protected readonly int _maxLevel;
        protected readonly VAction<IPerk> _change = new();
        protected T[] _buffs;

        public int MaxLevel => _maxLevel;

        protected ABuffs(int maxLevel) => _maxLevel = maxLevel;

        public Subscription Subscribe(Action<IPerk> action, bool instantGetValue = true)
        {
            for (int i = 0; instantGetValue & i < _buffs.Length; i++)
                action(_buffs[i].Current);

            return _change.Add(action);
        }
    }
}
