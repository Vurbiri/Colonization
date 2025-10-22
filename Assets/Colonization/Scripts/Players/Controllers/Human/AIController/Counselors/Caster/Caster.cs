using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private partial class Caster : Counselor
        {
            private static readonly CasterSettings s_settings;
            private static readonly ReadOnlyIdArray<AbilityTypeId, ReadOnlyArray<int>> s_weights;
            private static readonly HashSet<int> s_goodIds, s_badIds;

            static Caster()
            {
                s_settings = SettingsFile.Load<CasterSettings>();
                s_weights = new(s_settings.weightsEconomic, s_settings.weightsMilitary);
                s_goodIds = new(s_settings.goodIds); s_badIds = new(s_settings.badIds);

                s_settings.weightsEconomic = null; s_settings.weightsMilitary = null;
                s_settings.goodIds = null; s_badIds = null;
            }

            private readonly Casts _casts = new();
            private readonly Leveling _leveling = new();
            private readonly Cast[] _current = new Cast[(EconomicSpellId.Count + MilitarySpellId.Count) / s_settings.spellDivider];

            public Caster(AIController parent) : base(parent)
            {
                Order.Create(this); Healing.Create(this); Blessing.Create(this); Wrath.Create(this); Summon.Create(this); Transmutation.Create(this); Sacrifice.Create(this);
                BloodTrade.Create(this); Spying.Create(this); WallBuild.Create(this); Marauding.Create(this); RoadDemolition.Create(this); SwapId.Create(this); Zeal.Create(this);

                PerkTrees.GetProgress(EconomicSpellId.Type).Subscribe(OnEconomicProgress);
                PerkTrees.GetProgress(MilitarySpellId.Type).Subscribe(OnMilitaryProgress);
            }

            public override IEnumerator Execution_Cn()
            {
                int count = _casts.Count / s_settings.spellDivider;
                for (int i = 0; i < count; i++)
                    _current[i] = _casts.Extract();

                Cast current;
                for (int i = 0; i < count; i++)
                {
                    current = _current[i]; _current[i] = null;

                    Log.Info($"[Caster] Player {HumanId} current cast [{current}]");
                    yield return current.TryCasting_Cn();

                    _casts.Add(current);
                    yield return s_waitRealtime.Restart();
                }

                yield break;
            }

            private void OnEconomicProgress(int progress) => OnLeveling(EconomicSpellId.Type, PerkTree.ProgressToLevel(progress));
            private void OnMilitaryProgress(int progress) => OnLeveling(MilitarySpellId.Type, PerkTree.ProgressToLevel(progress));

            [Impl(256)] private void OnLeveling(int type, int level)
            {
                while (level >= 0 && _leveling.TryGet(type, level--, out Cast cast))
                    _casts.Add(cast);
            }

            #region Nested: Casts, Leveling
            // **********************************************************
            private class Casts : WeightsList<Cast>
            {
                public Casts() : base(null, EconomicSpellId.Count + MilitarySpellId.Count) { }

                [Impl(256)] public void Add(Cast cast) => base.Add(cast, cast.Weight);
            }
            // **********************************************************
            private class Leveling
            {
                private readonly Cast[][] _casts = { new Cast[EconomicSpellId.Count], new Cast[MilitarySpellId.Count] };

                [Impl(256)] public void Add(Cast cast) => _casts[cast.Type][cast.Id] = cast;

                [Impl(256)] public bool TryGet(int type, int level, out Cast cast)
                {
                    cast = _casts[type][level];
                    _casts[type][level] = null;

                    return cast != null;
                }
            }
            // **********************************************************
            #endregion
        }
    }
}
