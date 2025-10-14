using System;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Characteristics
{
    public class PerkTree : IReactive<Perk>, IReactive<HashSet<int>[]>
    {
        public const int MIN_LEVEL = 0, MAX_LEVEL = 6;
        public const int MIN_PROGRESS = 0, MAX_PROGRESS = MAX_LEVEL * (MAX_LEVEL + 1);

        private readonly ReadOnlyArray<ReadOnlyArray<Perk>> _perks;
        private readonly RInt[] _progress = new RInt[AbilityTypeId.Count];
        private readonly HashSet<int>[] _learnedPerks = new HashSet<int>[AbilityTypeId.Count];
        private readonly VAction<Perk> _eventPerk = new();
        private readonly VAction<HashSet<int>[]> _eventHashSet = new();

        public Perk this[int typePerkId, int perkId] { [Impl(256)] get => _perks[typePerkId][perkId]; }

        #region Constructors
        private PerkTree(PerksScriptable perks)
        {
            _perks = perks;
            for (int t = 0; t < AbilityTypeId.Count; t++)
            {
                 _learnedPerks[t] = new(EconomicPerksId.Count);
                _progress[t] = new(MIN_PROGRESS);
            }
        }
        private PerkTree(PerksScriptable perks, int[][] learnedPerks)
        {
            _perks = perks;

            int[] learned;
            for (int t = 0, progress = 0; t < AbilityTypeId.Count; t++, progress = 0)
            {
                learned = learnedPerks[t];
                _learnedPerks[t] = new(learned);
                for (int i = learned.Length - 1; i >= 0; i--)
                    progress += _perks[t][learned[i]].Cost;
                _progress[t] = new(Math.Min(progress, MAX_PROGRESS));
            }
        }
        [Impl(256)] public static PerkTree Create(Player.Settings settings, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.perks != null) 
                return new(settings.perks, loadData.perks);
            return new(settings.perks);
        }
        #endregion

        [Impl(256)] public static int ProgressToLevel(int progress) => MathI.Sqrt(1 + (progress << 2)) - 1 >> 1;

        [Impl(256)] public RInt GetProgress(int typePerkId) => _progress[typePerkId];
        [Impl(256)] public int GetLevel(int typePerkId) => ProgressToLevel(_progress[typePerkId]);
        
        [Impl(256)] public bool IsPerkLearned(int typePerkId, int perkId) => _learnedPerks[typePerkId].Contains(perkId);
        [Impl(256)] public bool IsPerkLearned(Perk perk) => _learnedPerks[perk.Type].Contains(perk.Id);

        [Impl(256)] public bool IsAllTreeLearned(int typePerkId) => _learnedPerks[typePerkId].Count == AbilityTypeId.PerksCount[typePerkId];
        [Impl(256)] public bool IsAllLearned() => _learnedPerks[EconomicPerksId.Type].Count == EconomicPerksId.Count & _learnedPerks[MilitaryPerksId.Type].Count == MilitaryPerksId.Count;

        #region Subscribe
        public Subscription Subscribe(Action<Perk> action, bool instantGetValue = true)
        {
            for (int type = 0; instantGetValue & type < AbilityTypeId.Count; type++)
                foreach (int id in _learnedPerks[type]) 
                    action(_perks[type][id]);

            return _eventPerk.Add(action);
        }
        [Impl(256)] public Subscription Subscribe(Action<HashSet<int>[]> action, bool instantGetValue = true)
        {
            return _eventHashSet.Add(action, instantGetValue, _learnedPerks);
        }
        #endregion

        public bool GetNotLearned(int typePerk, int idPerk, out Perk perk)
        {
            perk = _perks[typePerk][idPerk];
            return !_learnedPerks[typePerk].Contains(idPerk);
        }

        [Impl(256)] public void Learn(int typePerk, int idPerk) => Learn(_perks[typePerk][idPerk]);
        public void Learn(Perk perk)
        {
            _learnedPerks[perk.Type].Add(perk.Id);
            _progress[perk.Type].Value = Math.Min(_progress[perk.Type] + perk.Cost, MAX_PROGRESS);

            _eventPerk.Invoke(perk);
            _eventHashSet.Invoke(_learnedPerks);
        }
    }
}
