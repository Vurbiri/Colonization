using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private class Scientist : Counselor
        {
            private const int COUNT = PerkTree.MAX_LEVEL - 1;
            private static readonly ScientistSettings s_settings;

            private readonly Perks _perks = new();
            private readonly Dictionary<int, List<Perk>>[] _levelingPerks = { new(COUNT), new(COUNT) };
            private Perk _perk;

            static Scientist() => s_settings = SettingsFile.Load<ScientistSettings>();

            public Scientist(AIController parent) : base(parent)
            {
            }

            public override IEnumerator Init_Cn()
            {
                Create(AbilityTypeId.Economic);
                yield return null;
                Create(AbilityTypeId.Military);
                yield return null;

                Log.Info($"[{PlayerId.PositiveNames_Ed[Id]}]");
                Log.Info(_perks.ToString());
                Log.Info("=========================================");
            }

            public override IEnumerator Planning_Cn()
            {
                _perk ??= _perks.Extract();
                yield break;
            }

            public override IEnumerator Execution_Cn()
            {
                if (_perk != null && Resources[CurrencyId.Blood] >= _perk.Cost)
                {
                    Log.Info($"[{PlayerId.PositiveNames_Ed[Id]}] -> {AbilityTypeId.Names_Ed[_perk.Type]}.{(_perk.Type == AbilityTypeId.Economic ? EconomicPerksId.Names_Ed : MilitaryPerksId.Names_Ed)[_perk.Id]}");
                    
                    Human.BuyPerk(_perk);
                    TryAddPerks(_perk.Type);
                    _perk = null;
                }

                yield return s_waitRealtime.Restart();
            }

            private void Create(int type)
            {
                if (PerkTree.IsAllTreeLearned(type))
                    return;
                
                var weights = s_settings.weights[type];
                int shift = s_settings.specialization[type] == Id ? s_settings.shift : 0;
                var leveling = _levelingPerks[type];
                int level = PerkTree.GetLevel(type);

                Perk perk; List<Perk> perks;
                for(int i = 0; i < weights.Count; i++)
                {
                    if(PerkTree.GetNotLearned(type, i, out perk))
                    {
                        if(perk.Level > level)
                        {
                            if (!leveling.TryGetValue(perk.Level, out perks))
                                leveling.Add(perk.Level, perks = new(COUNT));

                            perks.Add(perk);
                        }
                        else
                        {
                            _perks.Add(perk, weights[i] << shift);
                        }
                    }
                }
            }

            private void TryAddPerks(int type)
            {
                int count = _perks.Count;
                var leveling = _levelingPerks[type];
                int level = PerkTree.GetLevel(type);

                while (leveling.TryGetValue(level, out List<Perk> perks))
                {
                    var weights = s_settings.weights[type];
                    int shift = s_settings.specialization[type] == Id ? s_settings.shift : 0;
                    Perk perk;

                    leveling.Remove(level);
                    for (int i = perks.Count - 1; i >= 0; i--)
                    {
                        perk = perks[i];
                        _perks.Add(perk, weights[perk.Id] << shift);
                    }

                    level--;
                }

                Log.Info($"[{PlayerId.PositiveNames_Ed[Id]}] perks {count} -> {_perks.Count}");
            }

            private class Perks : WeightsList<Perk>
            {
                public Perks() : base(null) { }
            }
        }
    }
}
