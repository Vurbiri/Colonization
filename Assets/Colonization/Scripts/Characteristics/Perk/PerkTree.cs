using System;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class PerkTree : IReactive<Perk>, IReactive<HashSet<int>[]>
    {
        public const int MIN_LEVEL = 0, MAX_LEVEL = 6;
        public const int MIN_PROGRESS = 0, MAX_PROGRESS = MAX_LEVEL * (MAX_LEVEL + 1);
        
        private static readonly ReadOnlyArray<ReadOnlyArray<Perk>> s_perks;

        private readonly RInt[] _progress = new RInt[AbilityTypeId.Count];
        private readonly HashSet<int>[] _learnedPerks = new HashSet<int>[AbilityTypeId.Count];
        private readonly VAction<Perk> _eventPerk = new();
        private readonly VAction<HashSet<int>[]> _eventHashSet = new();

        public Perk this[int typePerkId, int perkId] { [Impl(256)] get => s_perks[typePerkId][perkId]; }

        #region Constructors
        static PerkTree()
        {
            var scriptable = UnityEngine.Resources.Load<PerksScriptable>(string.Concat(SettingsFile.FOLDER, PerksScriptable.NAME));
            s_perks = scriptable; scriptable.Dispose();
        }

        private PerkTree()
        {
            for (int t = 0; t < AbilityTypeId.Count; t++)
            {
                 _learnedPerks[t] = new(AbilityTypeId.PerksCount[t]);
                _progress[t] = new(MIN_PROGRESS);
            }
        }
        private PerkTree(int[][] learnedPerks)
        {
            int[] learned;
            for (int t = 0, progress = 0; t < AbilityTypeId.Count; t++, progress = 0)
            {
                learned = learnedPerks[t];
                _learnedPerks[t] = new(learned);
                for (int i = learned.Length - 1; i >= 0; i--)
                    progress += s_perks[t][learned[i]].Cost;
                _progress[t] = new(Math.Min(progress, MAX_PROGRESS));
            }
        }
        [Impl(256)] public static PerkTree Create(Player.Settings settings, HumanLoadData loadData)
        {
            if (loadData.isLoaded & loadData.perks != null) 
                return new(loadData.perks);
            return new();
        }
        #endregion

        [Impl(256)] public static int ProgressToLevel(int progress) => MathI.Sqrt(1 + (progress << 2)) - 1 >> 1;

        [Impl(256)] public RInt GetProgress(int typePerkId) => _progress[typePerkId];
        [Impl(256)] public int GetLevel(int typePerkId) => ProgressToLevel(_progress[typePerkId]);
        
        [Impl(256)] public bool IsPerkLearned(int typePerkId, int perkId) => _learnedPerks[typePerkId].Contains(perkId);
        [Impl(256)] public bool IsPerkLearned(Perk perk) => _learnedPerks[perk.Type].Contains(perk.Id);

        [Impl(256)] public int PerkLearned(int typePerkId) => _learnedPerks[typePerkId].Count;

        [Impl(256)] public bool IsAllTreeLearned(int typePerkId) => _learnedPerks[typePerkId].Count == AbilityTypeId.PerksCount[typePerkId];
        [Impl(256)] public bool IsAllLearned() => _learnedPerks[EconomicPerksId.Type].Count == EconomicPerksId.Count & _learnedPerks[MilitaryPerksId.Type].Count == MilitaryPerksId.Count;

        #region Subscribe
        public Subscription Subscribe(Action<Perk> action, bool instantGetValue = true)
        {
            for (int type = 0; instantGetValue & type < AbilityTypeId.Count; type++)
                foreach (int id in _learnedPerks[type]) 
                    action(s_perks[type][id]);

            return _eventPerk.Add(action);
        }
        [Impl(256)] public Subscription Subscribe(Action<HashSet<int>[]> action, bool instantGetValue = true)
        {
            return _eventHashSet.Add(action, _learnedPerks, instantGetValue);
        }
        #endregion

        public bool GetNotLearned(int typePerk, int idPerk, out Perk perk)
        {
            perk = s_perks[typePerk][idPerk];
            return !_learnedPerks[typePerk].Contains(idPerk);
        }

        [Impl(256)] public void Learn(int typePerk, int idPerk) => Learn(s_perks[typePerk][idPerk]);
        public void Learn(Perk perk)
        {
            _learnedPerks[perk.Type].Add(perk.Id);
            _progress[perk.Type].Value = Math.Min(_progress[perk.Type] + perk.Cost, MAX_PROGRESS);

            _eventPerk.Invoke(perk);
            _eventHashSet.Invoke(_learnedPerks);
        }
    }
}
