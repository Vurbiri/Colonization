//Assets\Colonization\Scripts\Characteristics\Buffs\Demon\DemonBuff.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class DemonBuff : ABuff<DemonBuffSettings>
    {
        private readonly int _levelUP;

        public DemonBuff(Subscriber<IPerk> subscriber, DemonBuffSettings settings) : base(subscriber, settings) => _levelUP = settings.levelUP;

        public DemonBuff(Subscriber<IPerk> subscriber, DemonBuffSettings settings, int level) : base(subscriber, settings, settings.value * level / settings.levelUP)
        {
            _levelUP = settings.levelUP;
        }

        public void Next(int level)
        {
            if (level % _levelUP != 0)
                return;

            _current.Add(_base);
            _subscriber.Invoke(_base);
        }

        
    }
}
