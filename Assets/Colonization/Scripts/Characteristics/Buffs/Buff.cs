//Assets\Colonization\Scripts\Characteristics\Buffs\Buff.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class Buff : ABuff
    {
        public Buff(Subscription<IPerk> subscriber, BuffSettings settings) : base(subscriber, settings) { }

        public Buff(Subscription<IPerk> subscriber, BuffSettings settings, int level) : base(subscriber, settings, settings.value * level) { }

        public void Next()
        {
            _current.Add(_base);
            _subscriber.Invoke(_base);
        }
    }
}
