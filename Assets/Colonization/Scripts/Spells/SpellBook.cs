using System.Collections.ObjectModel;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public class SpellCosts : ReadOnlyCollection<ReadOnlyCollection<int>>
    {
        public SpellCosts(ReadOnlyCollection<int> e, ReadOnlyCollection<int> m) : base(new ReadOnlyCollection<int>[] {e, m}) {}
    }


    public partial class SpellBook
	{
        private static readonly SpellsSettings s_settings;
        private static readonly SpellCosts s_costs;

        private static readonly Human[] s_humans = new Human[PlayerId.HumansCount];
        private static readonly Roads[] s_roads = new Roads[PlayerId.HumansCount];
        private static readonly ReadOnlyReactiveSet<Actor>[] s_actors = new ReactiveSet<Actor>[PlayerId.Count];

        
        private static Coroutines s_coroutines;
        private static CameraController s_cameraController;

        private readonly ICurrency mana;
        private readonly ASpell[][] _spells = { new ASpell[EconomicSpellId.Count], new ASpell[MilitarySpellId.Count] };

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
            s_roads[id] = human.Roads;
            s_actors[id] = human.Warriors;

            int i = 0;
            _spells[0][i++] = new Order(id);
        }

        public bool TryCast(int type, int id, SpellParam param, out int cost)
        {
            cost = s_costs[type][id];
            return mana.Value >= cost && _spells[type][id].Cast(param);
        }

        public static void Init(Coroutines coroutines, CameraController cameraController)
        {
            s_coroutines = coroutines;
            s_cameraController = cameraController;
        }
        
        public static void AddSatan(Satan satan)
        {
            s_actors[PlayerId.Satan] = satan.Demons;
        }

        public static void Clear()
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
            {
                s_humans[i] = null; s_roads[i] = null; s_actors[i] = null;
            }
            s_actors[PlayerId.Satan] = null;
            s_coroutines = null; s_cameraController = null;
        }
    }
}
