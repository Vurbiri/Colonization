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

                Create(AbilityTypeId.Economic);
                Create(AbilityTypeId.Military);

                // ==== Local ====
                void Create(int type)
                {
                    if (!PerkTrees.IsAllTreeLearned(type))
                    {
                        int level = PerkTrees.GetLevel(type);
                        int count = AbilityTypeId.PerksCount[type];

                        for (int id = 0; id < count; id++)
                        {
                            if (PerkTrees.GetNotLearned(type, id, out Perk perk))
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

                if (_perk != null && Resources[CurrencyId.Blood] >= _perk.Cost)
                {
                    Human.BuyPerk(_perk);

                    int progress = PerkTrees.GetProgress(_perk.Type);
                    if (progress < PerkTree.MAX_PROGRESS && _leveling.TryGet(_perk.Type, PerkTree.ProgressToLevel(progress), out List<Perk> perks))
                        _perks.Add(_perk.Type, perks);

                    Log.Info($"[Scientist] Player {HumanId} learned a perk [{_perk.Type}].[{_perk.Id}]");

                    _perk = null;
                }

                yield return s_waitRealtime.Restart();
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
