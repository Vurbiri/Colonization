using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public abstract class AHumanController : Human,  IPlayerController
	{
        protected AHumanController(int playerId, Settings settings) : base(playerId, settings) { }

        // TSET !!!!!!!!!!!!!!
        public void SpawnTest(int id, int count)
        {
            UnityEngine.Debug.Log("SpawnTest");
            Hexagon hexagon;
            for (int i = 0; i < count; i++)
            {
                while (!(hexagon = GameContainer.Hexagons[HEX.NEARS.Random]).CanWarriorEnter) ;
                Warrior warrior = _spawner.Create(id, hexagon);
                warrior.IsPersonTurn = _isPerson;

                _actors.Add(warrior);
            }
        }
        public void SpawnTest(int id, Key key)
        {
            UnityEngine.Debug.Log("SpawnTest");
            Hexagon hexagon;
            if ((hexagon = GameContainer.Hexagons[key]).CanWarriorEnter)
            {
                Warrior warrior = _spawner.Create(id, hexagon);
                warrior.IsPersonTurn = _isPerson;

                _actors.Add(warrior);
            }
        }

        public void ActorKill(Id<ActorTypeId> type, int id)
        {
            if (type == ActorTypeId.Demon)
            {
                GameContainer.Score.ForKillingDemon(_id, id);
                _resources.AddBlood(id);
            }
            else
            {
                GameContainer.Score.ForKillingWarrior(_id, id);
            }
        }

        public virtual void OnLanding() { }
        public virtual void OnEndLanding() { }

        public virtual void OnEndTurn()
        {
            int countBuffs = 0;
            CurrenciesLite profit = new();
            bool isArtefact = _abilities.IsTrue(HumanAbilityId.IsArtefact);
            foreach (var warrior in _actors)
            {
                if (warrior.IsMainProfit)
                    profit.IncrementMain(warrior.Hexagon.SurfaceId);
                if (isArtefact && warrior.IsAdvProfit)
                    countBuffs++;

                warrior.StatesUpdate();
                warrior.IsPersonTurn = false;
                warrior.Interactable = false;
            }

            _resources.Add(profit);
            _artefact.Next(countBuffs);
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            if (id == PlayerId.Satan)
                _resources.AddBlood(_edifices.ShrinePassiveProfit);

            if (hexId == CONST.GATE_ID)
            {
                _resources.AddBlood(_edifices.ShrineProfit);
                _resources.ClampMain();
                return;
            }

            if (_abilities.IsTrue(HumanAbilityId.IsFreeGroundRes))
                _resources.Add(GameContainer.Hexagons.FreeResources);

            _resources.Add(_edifices.ProfitFromEdifices(hexId));
        }

        public void OnStartTurn()
        {
            foreach (var warrior in _actors)
                warrior.EffectsUpdate();

            _exchange.Update();
        }

        public abstract void OnPlay();

    }
}
