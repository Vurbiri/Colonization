using System.Collections;

namespace Vurbiri.Colonization
{
	sealed public class SatanController : Satan,  IPlayerController
	{
        private readonly Game _game;
        private readonly Coroutines _coroutines;

        public SatanController(Game game, Storage.SatanStorage storage, Players.Settings settings) : base(storage, settings)
        {
            _game = game;
            _coroutines = settings.coroutines;
        }

        public void OnInit()
        {
            _coroutines.Run(OnInit_Cn());
        }

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

        private IEnumerator OnInit_Cn()
        {
            yield return null;
            _game.EndTurn();
        }
    }
}
