using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization
{
	public abstract class AHumanController : Human,  IPlayerController
	{
        protected readonly Hexagons _hexagons;

        protected AHumanController(int playerId, Storage.HumanStorage storage, Players.Settings settings) : base(playerId, storage, settings)
        {
            _hexagons = settings.hexagons;
        }

        public virtual void OnLanding() { }
        public virtual void OnEndLanding() { }

        public virtual void OnEndTurn()
        {
            int countBuffs = 0;
            CurrenciesLite profit = new();
            bool isArtefact = _abilities.IsTrue(HumanAbilityId.IsArtefact);
            foreach (var warrior in _warriors)
            {
                if (warrior.IsMainProfit)
                    profit.Increment(warrior.Hexagon.SurfaceId);
                if (isArtefact && warrior.IsAdvProfit)
                    countBuffs++;

                warrior.StatesUpdate();
                warrior.IsPlayerTurn = false;
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
                _resources.Add(_hexagons.FreeResources);

            _resources.Add(_edifices.ProfitFromEdifices(hexId));
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
