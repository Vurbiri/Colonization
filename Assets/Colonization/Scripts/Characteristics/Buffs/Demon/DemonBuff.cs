//Assets\Colonization\Scripts\Characteristics\Buffs\Demon\DemonBuff.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class DemonBuff : ABuff
    {
        private readonly int _levelUP;

        public DemonBuff(Subscription<IPerk> subscriber, DemonBuffSettings settings, int level) : base(subscriber, settings, settings.value * level / settings.levelUP)
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
