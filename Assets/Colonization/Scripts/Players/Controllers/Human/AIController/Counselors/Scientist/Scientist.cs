using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Scientist : Counselor
        {
            private static readonly ScientistSettings s_settings;

            private readonly Perks _perks;
            private readonly Leveling _leveling = new();
            private Perk _perk;

            static Scientist() => s_settings = SettingsFile.Load<ScientistSettings>();

            public Scientist(AIController parent) : base(parent)
            {
                _perks = new(Id);
            }

            public override IEnumerator Init_Cn()
            {
                Create(AbilityTypeId.Economic);
                yield return null;
                Create(AbilityTypeId.Military);
                yield return null;

                // ------ Local ----
                void Create(int type)
                {
                    if (!PerkTree.IsAllTreeLearned(type))
                    {
                        int level = PerkTree.GetLevel(type);
                        int count = AbilityTypeId.PerksCount[type];

                        for (int i = 0; i < count; i++)
                        {
                            if (PerkTree.GetNotLearned(type, i, out Perk perk))
                            {
                                if (perk.Level > level)
                                    _leveling.Add(type, perk);
                                else
                                    _perks.Add(type, perk);
                            }
                        }
                    }
                }
            }

            public override IEnumerator Planning_Cn()
            {
                _perks.Extract(ref _perk);
                yield break;
            }

            public override IEnumerator Execution_Cn()
            {
                if (_perk != null && Resources[CurrencyId.Blood] >= _perk.Cost)
                {
                    Human.BuyPerk(_perk);
                    Log.Info($"[Scientist] Player {Id} learned a perk [{_perk.Type}].[{_perk.Id}]");

                    int type = _perk.Type, progress = PerkTree.GetProgress(type);
                    if (progress < PerkTree.MAX_PROGRESS && _leveling.TryGet(type, PerkTree.ProgressToLevel(progress), out List<Perk> perks))
                    {
                        for (int i = perks.Count - 1; i >= 0; i--)
                            _perks.Add(type, perks[i]);
                    }
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

                public Perks(Id<PlayerId> playerId)
                {
                    if (s_settings.specialization[AbilityTypeId.Economic] == playerId)
                        _chance = Chance.MAX_CHANCE - s_settings.chance;
                    else if (s_settings.specialization[AbilityTypeId.Military] == playerId)
                        _chance = s_settings.chance;
                    else
                        _chance = 50;
                }

                [Impl(256)] public void Add(int type, Perk perk) => _perks[type].Add(perk, s_settings.weights[type][perk.Id]);

                public void Extract(ref Perk perk)
                {
                    if (perk == null)
                    {
                        int type = _chance.Select(AbilityTypeId.Economic, AbilityTypeId.Military);
                        perk = _perks[type].Extract();
                        perk ??= _perks[AbilityTypeId.Other(type)].Extract();
                    }
                }
            }
            // **********************************************************
            private class Leveling
            {
                private const int COUNT = PerkTree.MAX_LEVEL, MAX_IN_LINE = 5;

                private readonly List<Perk>[][] _perks = { new List<Perk>[COUNT], new List<Perk>[COUNT] };

                public void Add(int type, Perk perk)
                {
                    var perks = _perks[type][perk.Level];
                    if (perks == null)
                        _perks[type][perk.Level] = perks = new(MAX_IN_LINE);

                    perks.Add(perk);
                }

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
