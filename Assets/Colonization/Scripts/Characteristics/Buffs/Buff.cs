//Assets\Colonization\Scripts\Characteristics\Buffs\Buff.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class Buff : ABuff<BuffSettings>
    {
        private int _level;

        public int Level => _level;

        public Buff(Subscriber<IPerk> subscriber, BuffSettings settings) : base(subscriber, settings) { }

        public Buff(Subscriber<IPerk> subscriber, BuffSettings settings, int level) : base(subscriber, settings, settings.value * level) => _level = level;

        public void Next()
        {
            _level++;
            _current.Add(_base);
            _subscriber.Invoke(_base);
        }
    }
}
