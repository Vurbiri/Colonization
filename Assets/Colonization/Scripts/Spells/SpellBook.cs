using System.Collections.ObjectModel;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.TypeOfPerksId;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
	{
        private static readonly SpellsSettings s_settings;
        private static readonly SpellCosts s_costs;

        private static readonly Human[] s_humans = new Human[PlayerId.HumansCount];
        private static readonly ReadOnlyReactiveSet<Actor>[] s_actors = new ReactiveSet<Actor>[PlayerId.Count];
        private static readonly ASharedSpell[][] s_sharedSpells = { new ASharedSpell[EconomicSpellId.Count], new ASharedSpell[MilitarySpellId.Count] };

        private static Coroutines s_coroutines;
        private static CameraController s_cameraController;
        
        private readonly ICurrency mana;
        private readonly CurrenciesLite _resources = new();
        private readonly APlayerSpell[][] _spells = { new APlayerSpell[EconomicSpellId.Count], new APlayerSpell[MilitarySpellId.Count] };

        public static SpellCosts Costs => s_costs;

        static SpellBook()
        {
            s_settings = SettingsFile.Load<SpellsSettings>();
            s_costs = new(s_settings.economicCost.Values, s_settings.militaryCost.Values);
            s_settings.economicCost = null; s_settings.militaryCost = null;
        }

        public SpellBook(Human human)
        {
            int id = human.Id;
            mana = human.Resources.Get(CurrencyId.Mana);
            
            s_humans[id] = human;
            s_actors[id] = human.Warriors;
        }

        public CurrenciesLite Cast(int type, int id, SpellParam param)
        {
            _resources.Clear();

            int cost = s_costs[type][id];
            ASpell spell = s_sharedSpells[type][id]; spell ??= _spells[type][id];
            if (mana.Value >= cost && spell.Cast(param, _resources))
                _resources.Add(CurrencyId.Mana, -cost);

            return _resources;
        }

        public static void Init(Coroutines coroutines, CameraController cameraController)
        {
            s_coroutines = coroutines;
            s_cameraController = cameraController;

            Order.Create();

            BloodTrade.Create();
        }
        
        public static void AddSatan(Satan satan)
        {
            s_actors[PlayerId.Satan] = satan.Demons;
        }

        public static void Clear()
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                s_humans[i] = null; s_actors[i] = null;
            }
            s_actors[PlayerId.Satan] = null;
            s_coroutines = null; s_cameraController = null;

            for (int i = 0; i < EconomicSpellId.Count; i++)
                s_sharedSpells[Economic][i] = null;
            for (int i = 0; i < MilitarySpellId.Count; i++)
                s_sharedSpells[Military][i] = null;
        }
    }
    public class SpellCosts : ReadOnlyCollection<ReadOnlyCollection<int>>
    {
        public SpellCosts(ReadOnlyCollection<int> e, ReadOnlyCollection<int> m) : base(new ReadOnlyCollection<int>[] { e, m }) { }
    }
}
