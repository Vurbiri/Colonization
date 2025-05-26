using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public abstract class AHumanController : Human,  IPlayerController
	{
        protected AHumanController(Id<PlayerId> playerId, Storage.HumanStorage storage, Players.Settings settings) : base(playerId, storage, settings)
        {
        }

        public virtual void OnInit()
        {

        }

        public virtual void OnEndTurn()
        {
            int countBuffs = 0;
            CurrenciesLite profit = new();
            foreach (var warrior in _warriors)
            {
                if (warrior.IsMainProfit)
                    profit.Increment(warrior.Hexagon.SurfaceId);
                if (warrior.IsAdvProfit)
                    countBuffs++;

                warrior.StatesUpdate();
                warrior.IsPlayerTurn = false;
            }

            _resources.AddFrom(profit);
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
                _resources.AddFrom(Hexagons.FreeResources);

            _resources.AddFrom(_edifices.ProfitFromEdifices(hexId));
        }

        public void OnStartTurn()
        {
            foreach (var warrior in _warriors)
                warrior.EffectsUpdate();

            _exchange.Update();
        }

        public abstract void OnPlay();
    }
}
