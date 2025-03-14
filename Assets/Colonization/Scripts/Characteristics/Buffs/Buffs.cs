//Assets\Colonization\Scripts\Characteristics\Buffs\Buffs.cs
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class Buffs : ABuffs<Buff>, IReactive<IReadOnlyList<int>>
    {
        protected readonly int[] _levels;
        protected readonly Subscriber<IReadOnlyList<int>> _subscriberLevels = new();
        private RInt _rand;
        private int _current;
        
        public Buffs(IReadOnlyList<BuffSettings> settings)
        {
            _count = settings.Count;
            _rand = new(_count);
            _current = _rand;

            _levels = new int[_count];
            _buffs = new Buff[_count];
            for (int i = 0; i < _count; i++)
                _buffs[i] = new(_subscriber, settings[i]);
        }

        public Buffs(IReadOnlyList<BuffSettings> settings, int[] levels)
        {
            _count = settings.Count;
            _rand = new(_count);
            _current = _rand;

            _levels = levels;
            _buffs = new Buff[_count];
            for (int i = 0; i < _count; i++)
                _buffs[i] = new(_subscriber, settings[i], levels[i]);
        }

        public void Next(int count)
        {
            if (count <= 0) return;
            
            for (int i = 0; i < count; i++)
            {
                _current = (_current + _rand) % _count;
                _buffs[_current].Next();
                _levels[_current]++;
            }

            _subscriberLevels.Invoke(_levels);
        }

        public Unsubscriber Subscribe(System.Action<IReadOnlyList<int>> action, bool calling = true) => _subscriberLevels.Add(action, calling, _levels);

    }
}
