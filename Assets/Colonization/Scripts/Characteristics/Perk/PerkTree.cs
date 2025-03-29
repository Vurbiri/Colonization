//Assets\Colonization\Scripts\Characteristics\Perk\PerkTree.cs
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class PerkTree : IReactive<Perk>, IReactive<IEnumerable<IEnumerable<int>>>
    {
        private readonly IReadOnlyList<Perk>[] _perks = new IReadOnlyList<Perk>[TypePerksId.Count];
        private readonly RInt[] _levels = new RInt[TypePerksId.Count];
        private readonly HashSet<int>[] _learnedPerks = new HashSet<int>[TypePerksId.Count];
        private readonly Subscriber<Perk> _eventPerk = new();
        private readonly Subscriber<IEnumerable<IEnumerable<int>>> _eventHashSet = new();

        public IReactiveValue<int> EconomicLevel => _levels[TypePerksId.Economic];
        public IReactiveValue<int> MilitaryLevel => _levels[TypePerksId.Military];

        public ISubscriber<IEnumerable<IEnumerable<int>>> LearnedPerks => _eventHashSet;

        private PerkTree(EconomicPerksScriptable economicPerks, MilitaryPerksScriptable militaryPerks)
        {
            _perks[TypePerksId.Economic] = economicPerks.Perks;
            _perks[TypePerksId.Military] = militaryPerks.Perks;

            for (int i = 0; i < TypePerksId.Count; i++)
            {
                _learnedPerks[i] = new();
                _levels[i] = new();
            }
        }

        private PerkTree(EconomicPerksScriptable economicPerks, MilitaryPerksScriptable militaryPerks, int[][] perks)
        {
            _perks[TypePerksId.Economic] = economicPerks.Perks;
            _perks[TypePerksId.Military] = militaryPerks.Perks;

            for (int i = 0; i < TypePerksId.Count; i++)
            {
                _learnedPerks[i] = new(perks[i]);
                _levels[i] = new(LevelCalc(_learnedPerks[i]));
            }
        }

        public static PerkTree Create(Players.Settings settings, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.perks != null) 
                return new(settings.economicPerks, settings.militaryPerks, loadData.perks);
            return new(settings.economicPerks, settings.militaryPerks);
        }

        public Unsubscriber Subscribe(Action<Perk> action, bool calling = true)
        {
            for (int type = 0; calling & type < TypePerksId.Count; type++)
                foreach (int id in _learnedPerks[type]) action(_perks[type][id]);

            return _eventPerk.Add(action);
        }
        public Unsubscriber Subscribe(Action<IEnumerable<IEnumerable<int>>> action, bool calling = true) => _eventHashSet.Add(action, calling, _learnedPerks);

        public bool TryAdd(int typePerk, int idPerk, out int cost)
        {
            HashSet<int> perks = _learnedPerks[typePerk];

            if (!perks.Add(idPerk))
            {
                cost = 0; return false;
            }

            _levels[typePerk].Value = LevelCalc(perks);

            Perk perk = _perks[typePerk][idPerk];
            cost = perk.Cost;

            _eventPerk.Invoke(perk);
            _eventHashSet.Invoke(_learnedPerks);
            return true;
        }

        public void Dispose()
        {
            _eventPerk.Dispose();
            _eventHashSet.Dispose();
            for (int i = 0; i < TypePerksId.Count; i++)
                _levels[i].Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int LevelCalc(HashSet<int> perks) => (perks.Count + 1) >> 1;
    }
}
