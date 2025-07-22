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
            GameContainer.GameLoop.EndTurn().Start();
        }
        public void OnEndLanding() { }

        public void OnEndTurn()
        {
            int countBuffs = 0, balance = 0;
            foreach (var demon in _actors)
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
            if (hexId == CONST.GATE_ID)
                AddCurse(_states.curseProfit + _level * _states.curseProfitPerLevel);
        }

        public void OnStartTurn()
        {
            foreach (var demon in _actors)
                demon.EffectsUpdate(_states.gateDefense);
        }

        public void OnPlay()
        {
            AddCurse(CursePerTurn);
        }

    }
}
