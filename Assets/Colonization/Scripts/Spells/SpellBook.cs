using System.Collections.ObjectModel;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class SpellBook
	{
        private static readonly SpellsSettings s_settings;
        private static readonly SpellCosts s_costs;

        private static readonly Human[] s_humans = new Human[PlayerId.HumansCount];
        private static readonly ReadOnlyReactiveSet<Actor>[] s_actors = new ReactiveSet<Actor>[PlayerId.Count];
        private static readonly ASpell[][] s_spells = { s_economicSpells = new ASpell[EconomicSpellId.Count], s_militarySpells = new ASpell[MilitarySpellId.Count] };
        private static readonly ASpell[] s_economicSpells;
        private static readonly ASpell[] s_militarySpells;

        private static readonly RBool s_isCast = new(false);

        private static GameplayTriggerBus s_triggerBus;
        private static Coroutines s_coroutines;
        private static CameraController s_cameraController;
        private static Hexagons s_hexagons;

        private readonly ICurrency _mana;
        private readonly CurrenciesLite _resources = new();

        public static SpellCosts Costs => s_costs;

        public RBool IsCastReactive => s_isCast;
        public bool IsCast { get => s_isCast.Value; set => s_isCast.Value = value; }

        static SpellBook()
        {
            s_settings = SettingsFile.Load<SpellsSettings>();
            s_costs = new(s_settings.economicCost.Values, s_settings.militaryCost.Values);
            s_settings.economicCost = null; s_settings.militaryCost = null;
        }

        public SpellBook(Human human)
        {
            int id = human.Id;
            
            s_humans[id] = human;
            s_actors[id] = human.Warriors;

            _mana = human.Resources.Get(CurrencyId.Mana);
        }

        public void Cast(int type, int id, SpellParam param)
        {
            int cost = s_costs[type][id];

            if (_mana.Value >= cost & !s_isCast)
            {
                _resources.Clear();
                _resources.Set(CurrencyId.Mana, -cost);
                s_spells[type][id].Cast(param, _resources);
            }
        }

        public static void Init(GameplayInitObjects init)
        {
            s_coroutines = init.coroutines;
            s_triggerBus = init.triggerBus;
            s_cameraController = init.cameraController;
            s_hexagons = init.hexagons;

            Order.Create();
            SummonWarlock.Create(init.prices);

            BloodTrade.Create();
        }
        
        public static void AddSatan(Satan satan)
        {
            s_actors[PlayerId.Satan] = satan.Demons;
        }

        public static void Clear()
        {
            s_isCast.UnsubscribeAll();

            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                s_humans[i] = null; 
                s_actors[i] = null;
            }
            s_actors[PlayerId.Satan] = null;

            s_coroutines = null; s_triggerBus = null; s_cameraController = null; s_hexagons = null;

            for (int i = 0; i < EconomicSpellId.Count; i++)
                s_economicSpells[i].Clear();
            for (int i = 0; i < MilitarySpellId.Count; i++)
                s_militarySpells[i].Clear();
        }
    }

    public class SpellCosts : ReadOnlyCollection<ReadOnlyCollection<int>>
    {
        public SpellCosts(ReadOnlyCollection<int> e, ReadOnlyCollection<int> m) : base(new ReadOnlyCollection<int>[] { e, m }) { }
    }
}
