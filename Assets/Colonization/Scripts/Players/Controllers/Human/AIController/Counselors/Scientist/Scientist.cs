using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Scientist : Counselor
        {
            private static readonly ScientistSettings s_settings;

            private readonly Perks _perks;
            private readonly Leveling _leveling;
            private Perk _perk;

            static Scientist() => s_settings = SettingsFile.Load<ScientistSettings>();

            public Scientist(AIController parent) : base(parent)
            {
                _perks = new(parent._specialization);
                _leveling = new(PerkTrees.GetLevel(AbilityTypeId.Economic), PerkTrees.GetLevel(AbilityTypeId.Military));

                var freePerks = s_aiSettings.cheat.freePerks;

                Create(AbilityTypeId.Economic, freePerks.economic);
                Create(AbilityTypeId.Military, freePerks.military);

                freePerks.Subscribe(PerkTrees, FreeLearn);

                // ==== Local ====
                void Create(int type, int skip)
                {
                    if (!PerkTrees.IsAllTreeLearned(type))
                    {
                        int level = PerkTrees.GetLevel(type);
                        int count = AbilityTypeId.PerksCount[type];

                        for (int id = 0; id < count; id++)
                        {
                            if (id != skip && PerkTrees.GetNotLearned(type, id, out Perk perk))
                            {
                                if (perk.Level > level)
                                    _leveling.Add(perk);
                                else
                                    _perks.Add(perk);
                            }
                        }
                    }
                }
            }

            public override IEnumerator Execution_Cn()
            {
                _perks.Extract(ref _perk);

                if (_perk != null && Resources.Blood >= _perk.Cost)
                {
                    Human.BuyPerk(_perk);

                    int progress = PerkTrees.GetProgress(_perk.Type);
                    if (progress < PerkTree.MAX_PROGRESS && _leveling.TryGet(_perk.Type, PerkTree.ProgressToLevel(progress), out List<Perk> perks))
                        _perks.Add(_perk.Type, perks);
#if TEST_AI
                    UnityEngine.Debug.Log($"[Scientist] {HumanId} learned a perk {_perk}");
#endif
                    _perk = null;
                }

                yield return s_delayHalfSecond.Restart();
            }

            private void FreeLearn(TurnQueue turnQueue, int hexId)
            {
                var freePerks = s_aiSettings.cheat.freePerks;
                if(turnQueue.currentId == HumanId && turnQueue.turn >= freePerks.turn)
                {
#if TEST_AI
                    UnityEngine.Debug.Log($"[Scientist] {HumanId} FREE LEARN");
#endif
                    GameContainer.GameEvents.Unsubscribe(GameModeId.Play, FreeLearn);
                    Learn(EconomicPerksId.Type, freePerks.economic);
                    Learn(MilitaryPerksId.Type, freePerks.military);
                }

                // ================== Local ======================
                [Impl(256)] void Learn(int typePerkId, int perkId)
                {
                    if (PerkTrees.GetNotLearned(typePerkId, perkId, out Perk perk))
                        PerkTrees.Learn(perk);
                }
            }

            #region Nested: Perks, Leveling
            // **********************************************************
            private class Perks
            {
                private readonly WeightsList<Perk>[] _perks = { new(null), new(null) };
                private Chance _chance;

                public Perks(int specialization)
                {
                    _chance = specialization switch
                    {
                        AbilityTypeId.Economic => Chance.MAX_CHANCE - s_settings.chance,
                        AbilityTypeId.Military => s_settings.chance,
                        _ => 50
                    };
                }

                [Impl(256)] public void Add(Perk perk) => _perks[perk.Type].Add(perk, s_settings.weights[perk.Type][perk.Id]);

                public void Add(int type, List<Perk> addPerks)
                {
                    var perks = _perks[type]; Perk perk;
                    for (int i = addPerks.Count - 1; i >= 0; i--)
                    {
                        perk = addPerks[i];
                        perks.Add(perk, s_settings.weights[type][perk.Id]);
                    }
                        
                }

                [Impl(256)] public bool Remove(Perk perk) => _perks[perk.Type].Remove(perk);

                public void Extract(ref Perk perk)
                {
                    if (perk == null)
                    {
                        int type = _chance.Select(AbilityTypeId.Economic, AbilityTypeId.Military);
                        perk = _perks[type].Extract() ?? _perks[AbilityTypeId.Other(type)].Extract();
                    }
                }
            }
            // **********************************************************
            private class Leveling
            {
                private const int COUNT = PerkTree.MAX_LEVEL, MAX_IN_LINE = 5;

                private readonly List<Perk>[][] _perks = { new List<Perk>[COUNT], new List<Perk>[COUNT] };

                public Leveling(int economicLevel, int militaryLevel)
                {
                    Create(_perks[AbilityTypeId.Economic], economicLevel);
                    Create(_perks[AbilityTypeId.Military], militaryLevel);

                    // === Local ===
                    [Impl(256)] static void Create(List<Perk>[] perks, int level)
                    {
                        for (int i = level; i < COUNT; i++) 
                            perks[i] = new(MAX_IN_LINE);
                    }
                }

                [Impl(256)] public void Add(Perk perk) => _perks[perk.Type][perk.Level].Add(perk);
                [Impl(256)] public void Remove(Perk perk) => _perks[perk.Type][perk.Level]?.Remove(perk);

                public bool TryGet(int type, int level, out List<Perk> perks)
                {
                    perks = _perks[type][level];
                    _perks[type][level] = null;

                    return perks != null;
                }
            }
            // **********************************************************
            #endregion
        }
    }
}
