using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class DemonBuff : ABuff
    {
        private readonly int _levelUP;

        public DemonBuff(Subscription<IPerk> subscriber, BuffSettings settings, int level) : base(subscriber, settings, settings.value * level / settings.advance)
        {
            _levelUP = settings.advance;
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
