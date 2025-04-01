//Assets\Colonization\Scripts\Characteristics\Buffs\Buffs.cs
using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class Buffs : ABuffs<Buff>, IReactive<IReadOnlyList<int>>
    {
        private readonly int[] _levels;
        private readonly Subscriber<IReadOnlyList<int>> _subscriberLevels = new();
        private IndexRnd _rIndex;

        private Buffs(IReadOnlyList<BuffSettings> settings)
        {
            int count = settings.Count;

            _rIndex = new(count); 
            _levels = new int[count];
            _buffs = new Buff[count];

            for (int i = 0; i < count; i++)
                _buffs[i] = new(_subscriber, settings[i]);
        }

        private Buffs(IReadOnlyList<BuffSettings> settings, int[] levels)
        {
            int count = settings.Count;

            _rIndex = new(count);
            _levels = new int[count];
            _buffs = new Buff[count];

            for (int i = 0; i < count; i++)
                _buffs[i] = new(_subscriber, settings[i], _levels[i] = levels[i]);
        }

        public static Buffs Create(IReadOnlyList<BuffSettings> settings, APlayerLoadData loadData)
        {
            if(loadData.isLoaded & loadData.artefact != null) 
                return new(settings, loadData.artefact);
            return new(settings);
        }

        public void Next(int count)
        {
            if (count <= 0) return;
            
            for (int i = 0; i < count; i++)
            {
                _rIndex.Next();
                _buffs[_rIndex.Current].Next();
                _levels[_rIndex.Current]++;
            }

            _subscriberLevels.Invoke(_levels);
        }

        public Unsubscriber Subscribe(System.Action<IReadOnlyList<int>> action, bool sendCallback = true) => _subscriberLevels.Add(action, sendCallback, _levels);

    }
}
