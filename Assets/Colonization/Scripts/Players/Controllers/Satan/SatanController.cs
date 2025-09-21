using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
	sealed public class SatanController : Satan,  IPlayerController
	{
        public SatanController(Settings settings) : base(settings)
        {
        }

        public void ActorKill(Id<ActorTypeId> type, int id)
        {
            UnityEngine.Debug.Log($"ActorKilling: {type}, {id}");
        }

        public void OnLanding()
        {
            GameContainer.GameLoop.EndTurn_Cn().Start();
        }
        public void OnEndLanding() { }

        public void OnEndTurn()
        {
            int countBuffs = 0, balance = 0;
            foreach (var demon in Actors)
            {
                if (demon.IsMainProfit)
                    balance += (demon.Id + 1);
                if (demon.IsAdvProfit)
                    countBuffs++;

                demon.StatesUpdate();
            }

            GameContainer.Balance.ForCurse(balance);
            _artefact.Next(countBuffs);
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            int progress;
            if (hexId == CONST.GATE_ID)
            {
                progress = _settings.cursePerTurnReward;
            }
            else
            {
                if (hexId > CONST.GATE_ID)
                    hexId = (CONST.GATE_ID << 1) - hexId;

                progress = _settings.cursePerTurnBase * hexId / CONST.GATE_ID;
            }

            _curse.Add(progress);

            int maxCurse = MaxCurse;
            if (_curse >= maxCurse)
            {
                _curse.Remove(maxCurse);
                _level.Increment();
            }

            _eventChanged.Invoke(this);
        }

        public void OnStartTurn()
        {
            foreach (var demon in Actors)
                demon.EffectsUpdate(_settings.gateDefense);
        }

        public void OnPlay()
        {

        }

    }
}
