//Assets\Colonization\Scripts\Characteristics\Buffs\Abstract\ABuffs.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuffs<T> : IReactive<IPerk> where T : ABuff
    {
        protected readonly Subscriber<IPerk> _subscriber = new();
        protected T[] _buffs;

        public Unsubscriber Subscribe(Action<IPerk> action, bool calling = true)
        {
            if (calling)
            {
                for (int i = 0; i < _buffs.Length; i++)
                    action(_buffs[i].Current);
            }

            return _subscriber.Add(action);
        }
    }
}
