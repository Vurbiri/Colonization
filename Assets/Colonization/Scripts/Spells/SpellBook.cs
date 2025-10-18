using System.Collections;
using Vurbiri.Collections;
using Vurbiri.EntryPoint;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
	{
        private static readonly SpellsSettings s_settings;
        private static readonly Costs s_costs;
        private static readonly Keys s_keys;

        private static readonly ASpell[][] s_spells = {new ASpell[EconomicSpellId.Count], new ASpell[MilitarySpellId.Count] };
        private static readonly VAction<bool> s_casting = new();
        private static bool s_isCasting;

        private static ReadOnlyArray<HumanController> Humans { [Impl(256)] get => GameContainer.Players.Humans; }

        public ASpell this[int type, int id] { [Impl(256)] get => s_spells[type][id]; }
        public ASpell this[SpellId spellId]  { [Impl(256)] get => s_spells[spellId.type][spellId.id]; }

        public static Costs Cost { [Impl(256)] get => s_costs; }

        public static bool IsCasting { [Impl(256)] get => s_isCasting; }
        public static Event<bool> EventCasting { [Impl(256)] get => s_casting; }
        public static IEnumerator WaitEndCasting { [Impl(256)] get; } = new WaitCasting();

        static SpellBook()
        {
            s_settings = SettingsFile.Load<SpellsSettings>();
            s_costs = new (s_settings.economicCost, s_settings.militaryCost);
            s_keys  = new (s_settings.economicKey , s_settings.militaryKey );
            s_settings.economicCost = null; s_settings.militaryCost = null;
            s_settings.economicKey  = null; s_settings.militaryKey  = null;
        }

        public void Cast(int type, int id, SpellParam param)
        {
            var spell = s_spells[type][id];
            if(spell.Prep(param)) 
                spell.Cast(param);
        }

        public void Cancel(int type, int id)
        {
            if(s_isCasting) s_spells[type][id].Cancel();
        }

        private static void StartCasting()
        {
            s_isCasting = true;
            s_casting.Invoke(true);
        }
        private static void EndCasting()
        {
            s_isCasting = false;
            s_casting.Invoke(false);
        }

        public static void Init()
        {
            Order.Create(); RandomHealing.Create(); BlessingOfIsland.Create(); WrathOfIsland.Create(); SummonWarlock.Create(); Transmutation.Create(); Sacrifice.Create();
            BloodTrade.Create(); Spying.Create(); WallBuild.Create(); Marauding.Create(); RoadDemolition.Create(); SwapId.Create(); Zeal.Create();

            Transition.OnExit.Add(Clear);
        }

        private static void Clear()
        {
            s_casting.Clear();
            s_isCasting = false;

            for (int t = 0, count; t < AbilityTypeId.Count; t++)
            {
                count = AbilityTypeId.SpellsCount[t];
                for (int i = 0; i < count; i++)
                {
                    s_spells[t][i].Clear();
                    s_spells[t][i] = null;
                }
            }
        }
    }

}
