//Assets\Colonization\Scripts\Characteristics\Buffs\Abstract\ABuffs.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ABuffs<T> : IReactive<IPerk> where T : ABuff
    {
        protected readonly Subscription<IPerk> _subscriber = new();
        protected T[] _buffs;

        public Unsubscription Subscribe(Action<IPerk> action, bool instantGetValue = true)
        {
            for (int i = 0; instantGetValue & i < _buffs.Length; i++)
                action(_buffs[i].Current);

            return _subscriber.Add(action);
        }
    }
}
