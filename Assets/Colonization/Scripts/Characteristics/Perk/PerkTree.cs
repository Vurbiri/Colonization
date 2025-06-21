using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class PerkTree : IReactive<Perk>, IReactive<HashSet<int>[]>
    {
        public const int MIN_LEVEL = 0, MAX_LEVEL = 6, PROGRESS_PER_LEVEL = 2;
        public const int MIN_PROGRESS = 0, MAX_PROGRESS = MAX_LEVEL * (MAX_LEVEL + 1);

        private readonly ReadOnlyCollection<Perk>[] _perks = new ReadOnlyCollection<Perk>[TypeOfPerksId.Count];
        private readonly RInt[] _progress = new RInt[TypeOfPerksId.Count];
        private readonly HashSet<int>[] _learnedPerks = new HashSet<int>[TypeOfPerksId.Count];
        private readonly Subscription<Perk> _eventPerk = new();
        private readonly Subscription<HashSet<int>[]> _eventHashSet = new();

        public Perk this[int typePerkId, int perkId] => _perks[typePerkId][perkId];
        
        #region Constructors
        private PerkTree(PerksScriptable perks)
        {
            for (int t = 0; t < TypeOfPerksId.Count; t++)
            {
                _perks[t] = perks[t];
                _learnedPerks[t] = new(EconomicPerksId.Count);
                _progress[t] = new(MIN_PROGRESS);
            }
        }
        private PerkTree(PerksScriptable perks, int[][] learnedPerks)
        {
            for (int t = 0, progress = 0; t < TypeOfPerksId.Count; t++, progress = 0)
            {
                _perks[t] = perks[t];
                _learnedPerks[t] = new(learnedPerks[t]);

                for (int i = learnedPerks[t].Length - 1; i >= 0; i--)
                    progress += _perks[t][i].Cost;

                _progress[t] = new(Math.Min(progress, MAX_PROGRESS));
            }
        }
        public static PerkTree Create(Players.Settings settings, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.perks != null) 
                return new(settings.perks, loadData.perks);
            return new(settings.perks);
        }
        #endregion

        public RInt GetProgress(int typePerkId) => _progress[typePerkId];
        public bool IsPerkLearned(int typePerkId, int perkId) => _learnedPerks[typePerkId].Contains(perkId);

        #region Subscribe
        public Unsubscription Subscribe(Action<Perk> action, bool instantGetValue = true)
        {
            for (int type = 0; instantGetValue & type < TypeOfPerksId.Count; type++)
                foreach (int id in _learnedPerks[type]) 
                    action(_perks[type][id]);

            return _eventPerk.Add(action);
        }
        public Unsubscription Subscribe(Action<HashSet<int>[]> action, bool instantGetValue = true)
        {
            return _eventHashSet.Add(action, instantGetValue, _learnedPerks);
        }
        #endregion

        public bool TryAdd(int typePerk, int idPerk, out int cost)
        {
            if (!_learnedPerks[typePerk].Add(idPerk))
            {
                cost = 0; 
                return false;
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
