using System.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public abstract class HumanController : Human,  IPlayerController
	{
        protected readonly WaitResultSource<bool> _waitGift = new();

        protected HumanController(int playerId, Settings settings) : base(playerId, settings) { }

        public void ActorKill(Id<ActorTypeId> type, int id)
        {
            if (type == ActorTypeId.Demon)
            {
                GameContainer.Score.ForKillingDemon(_id, id);
                _resources.AddBlood(id + 1);
            }
            else
            {
                GameContainer.Score.ForKillingWarrior(_id, id);
            }
        }

        public abstract WaitResult<bool> OnGift(int giver, MainCurrencies gift, string msg);

        public virtual void OnLanding() { }
        public virtual void OnEndLanding() { }

        public abstract void OnEndTurn();

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            if (id == PlayerId.Satan)
                _resources.AddBlood(_edifices.ShrinePassiveProfit);

            if (hexId == HEX.GATE)
            {
                _resources.AddBlood(_edifices.ShrineProfit);
                _resources.ClampMain();
                return;
            }

            if (_abilities.IsTrue(HumanAbilityId.IsFreeGroundRes))
                _resources.Add(GameContainer.Hexagons.FreeResources);

            _resources.Add(_edifices.ProfitFromEdifices(hexId));
        }

        public virtual void OnStartTurn()
        {
            foreach (var warrior in Actors)
                warrior.EffectsUpdate();

            _exchange.Update();
        }

        public abstract void OnPlay();

        protected IEnumerator OnEndTurn_Cn()
        {
            int countBuffs = 0;
            int mainProfit = _abilities[HumanAbilityId.WarriorProfit];
            bool isArtefact = _abilities.IsTrue(HumanAbilityId.IsArtefact);
            MainCurrencies profit = new();
            ReturnSignal returnSignal;

            GameContainer.InputController.Unselect();

            foreach (var warrior in Actors)
            {
                if (returnSignal = warrior.IsMainProfit)
                {
                    profit.Add(warrior.Hexagon.GetProfit(), mainProfit);
                    yield return returnSignal.signal;
                }
                if (isArtefact && (returnSignal = warrior.IsAdvProfit))
                {
                    countBuffs++;
                    yield return returnSignal.signal;
                }

                warrior.StatesUpdate();
                warrior.IsPersonTurn = false;
                warrior.Interactable = false;
            }

            _resources.Add(profit);
            _artefact.Next(countBuffs);

            GameContainer.GameLoop.StartTurn();
        }
    }
}
