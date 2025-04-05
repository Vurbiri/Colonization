//Assets\Colonization\Scripts\Characteristics\Perk\PerkTree.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class PerkTree : IReactive<Perk>, IReactive<IEnumerable<IEnumerable<int>>>
    {
        public const int MIN_LEVEL = 0, MAX_LEVEL = 6, RATIO_PROGRESS_PER_LEVEL = 2;
        public const int MIN_PROGRESS = 0, MAX_PROGRESS = RATIO_PROGRESS_PER_LEVEL * MAX_LEVEL * (MAX_LEVEL + 1) >> 1;

        private readonly IReadOnlyList<Perk>[] _perks = new IReadOnlyList<Perk>[TypePerksId.Count];
        private readonly RInt[] _progress = new RInt[TypePerksId.Count];
        private readonly HashSet<int>[] _learnedPerks = new HashSet<int>[TypePerksId.Count];
        private readonly Signer<Perk> _eventPerk = new();
        private readonly Signer<IEnumerable<IEnumerable<int>>> _eventHashSet = new();

        public IReactiveValue<int> EconomicProgress => _progress[TypePerksId.Economic];
        public IReactiveValue<int> MilitaryProgress => _progress[TypePerksId.Military];

        private PerkTree(EconomicPerksScriptable economicPerks, MilitaryPerksScriptable militaryPerks)
        {
            _perks[TypePerksId.Economic] = economicPerks.Perks;
            _perks[TypePerksId.Military] = militaryPerks.Perks;

            for (int t = 0; t < TypePerksId.Count; t++)
            {
                _learnedPerks[t] = new();
                _progress[t] = new(MIN_PROGRESS);
            }
        }

        private PerkTree(EconomicPerksScriptable economicPerks, MilitaryPerksScriptable militaryPerks, int[][] perks)
        {
            _perks[TypePerksId.Economic] = economicPerks.Perks;
            _perks[TypePerksId.Military] = militaryPerks.Perks;

            for (int t = 0, progress = 0; t < TypePerksId.Count; t++, progress = 0)
            {
                _learnedPerks[t] = new(perks[t]);

                for (int i = perks[t].Length - 1; i >= 0; i--)
                    progress += _perks[t][i].Cost;

                _progress[t] = new(Math.Min(progress, MAX_PROGRESS));
            }
        }

        public static PerkTree Create(Players.Settings settings, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.perks != null) 
                return new(settings.economicPerks, settings.militaryPerks, loadData.perks);
            return new(settings.economicPerks, settings.militaryPerks);
        }

        public Unsubscriber Subscribe(Action<Perk> action, bool instantGetValue = true)
        {
            for (int type = 0; instantGetValue & type < TypePerksId.Count; type++)
                foreach (int id in _learnedPerks[type]) action(_perks[type][id]);

            return _eventPerk.Add(action);
        }
        public Unsubscriber Subscribe(Action<IEnumerable<IEnumerable<int>>> action, bool instantGetValue = true)
        {
            return _eventHashSet.Add(action, instantGetValue, _learnedPerks);
        }

        public bool TryAdd(int typePerk, int idPerk, out int cost)
        {
            if (!_learnedPerks[typePerk].Add(idPerk))
            {
                cost = 0; return false;
            }
            
            Perk perk = _perks[typePerk][idPerk];
            cost = perk.Cost;

            if(_progress[typePerk] < MAX_PROGRESS)
                _progress[typePerk].Value = Math.Min(_progress[typePerk] + cost, MAX_PROGRESS);

            _eventPerk.Invoke(perk);
            _eventHashSet.Invoke(_learnedPerks);
            return true;
        }
    }
}
