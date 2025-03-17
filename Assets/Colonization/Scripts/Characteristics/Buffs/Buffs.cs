//Assets\Colonization\Scripts\Characteristics\Buffs\Buffs.cs
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class Buffs : ABuffs<Buff>, IReactive<IReadOnlyList<int>>
    {
        private readonly int[] _levels;
        private readonly Subscriber<IReadOnlyList<int>> _subscriberLevels = new();
        private IndexRnd _rIndex;
        
        public Buffs(IReadOnlyList<BuffSettings> settings)
        {
            int count = settings.Count;

            _rIndex = new(count); 
            _levels = new int[count];
            _buffs = new Buff[count];

            for (int i = 0; i < count; i++)
                _buffs[i] = new(_subscriber, settings[i]);
        }

        public Buffs(IReadOnlyList<BuffSettings> settings, IReadOnlyList<int> levels)
        {
            int count = settings.Count;

            _rIndex = new(count);
            _levels = new int[count];
            _buffs = new Buff[count];

            for (int i = 0; i < count; i++)
                _buffs[i] = new(_subscriber, settings[i], _levels[i] = levels[i]);
        }

        public void Next(int count)
        {
            if (count <= 0) return;
            
            for (int i = 0; i < count; i++)
            {
                _rIndex.Next();
                _buffs[_rIndex].Next();
                _levels[_rIndex]++;
            }

            _subscriberLevels.Invoke(_levels);
        }

        public Unsubscriber Subscribe(System.Action<IReadOnlyList<int>> action, bool calling = true) => _subscriberLevels.Add(action, calling, _levels);

    }
}
