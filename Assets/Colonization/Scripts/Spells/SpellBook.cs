using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
	{
        private static readonly SpellsSettings s_settings;
        private static readonly int[][] s_costs;
        private static readonly string[][] s_keys;

        private static readonly Human[] s_humans = new Human[PlayerId.HumansCount];
        private static readonly ReadOnlyReactiveSet<Actor>[] s_actors = new ReactiveSet<Actor>[PlayerId.Count];

        private static readonly ASpell[][] s_spells = { s_economicSpells = new ASpell[EconomicSpellId.Count], s_militarySpells = new ASpell[MilitarySpellId.Count] };
        private static readonly ASpell[] s_economicSpells;
        private static readonly ASpell[] s_militarySpells;

        private static readonly RBool s_isCast = new(false);
        private static SharedSFXUser s_sfxUser;

        public ASpell this[int type, int id] => s_spells[type][id];
        public ASpell this[SpellId spellId] => s_spells[spellId.type][spellId.id];

        public RBool IsCast => s_isCast;

        static SpellBook()
        {
            s_settings = SettingsFile.Load<SpellsSettings>();
            s_costs = new int[][] { s_settings.economicCost.Values, s_settings.militaryCost.Values };
            s_keys  = new string[][] { s_settings.economicKey.Values, s_settings.militaryKey.Values };
            s_settings.economicCost = null; s_settings.militaryCost = null;
            s_settings.economicKey = null; s_settings.militaryKey = null;
        }

        public SpellBook(Human human)
        {
            int id = human.Id;
            
            s_humans[id] = human;
            s_actors[id] = human.Actors;
        }

        public void Cast(int type, int id, SpellParam param)
        {
            var spell = s_spells[type][id];
            if(spell.Prep(param)) 
                spell.Cast(param);
        }

        public void Cancel(int type, int id)
        {
            if(s_isCast) s_spells[type][id].Cancel();
        }

        public static void Init()
        {
            s_sfxUser = new();

            Order.Create(); RandomHealing.Create(); BlessingOfIsland.Create(); WrathOfIsland.Create(); SummonWarlock.Create(); Transmutation.Create(); Sacrifice.Create();
            BloodTrade.Create(); Spying.Create(); WallBuild.Create(); Marauding.Create(); RoadDemolition.Create(); SwapId.Create(); Zeal.Create();
        }
        
        public static void AddSatanActors(ReadOnlyReactiveSet<Actor> actors) => s_actors[PlayerId.Satan] = actors;

        public static void Clear()
        {
            s_sfxUser = null;
            s_isCast.UnsubscribeAll(); s_isCast.SilentValue = false;

            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                s_humans[i] = null; s_actors[i] = null;
            }
            s_actors[PlayerId.Satan] = null;

            for (int i = 0; i < EconomicSpellId.Count; i++)
                s_economicSpells[i].Clear(EconomicSpellId.Type, i);
            for (int i = 0; i < MilitarySpellId.Count; i++)
                s_militarySpells[i].Clear(MilitarySpellId.Type, i);
        }
    }

}
