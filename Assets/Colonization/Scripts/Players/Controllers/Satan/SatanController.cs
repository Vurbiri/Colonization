using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization
{
	sealed public class SatanController : Satan,  IPlayerController
	{
        private readonly GameLoop _game;
        private readonly Coroutines _coroutines;
        private readonly CameraController _cameraController;

        public SatanController(GameLoop game, Storage.SatanStorage storage, Players.Settings settings) : base(storage, settings)
        {
            _game = game;
            _coroutines = settings.coroutines;
            _cameraController = settings.cameraController;
        }

        public void OnLanding()
        {
            _coroutines.Run(_game.EndTurn());
        }
        public void OnEndLanding() { }

        public void OnEndTurn()
        {
            int countBuffs = 0, balance = 0;
            foreach (var demon in _demons)
            {
                if (demon.IsMainProfit)
                    balance += (demon.Id + 1);
                if (demon.IsAdvProfit)
                    countBuffs++;

                demon.StatesUpdate();
            }

            _balance.DemonCurse(balance);
            _artefact.Next(countBuffs);
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            if (hexId == CONST.GATE_ID)
                AddCurse(_states.curseProfit + _level * _states.curseProfitPerLevel);
        }

        public void OnStartTurn()
        {
            foreach (var demon in _demons)
                demon.EffectsUpdate(_states.gateDefense);
        }

        public void OnPlay()
        {
            AddCurse(CursePerTurn);
        }

    }
}
