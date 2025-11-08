using System.Collections;

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
            GameContainer.GameLoop.EndTurn();
        }
        public void OnEndLanding() { }

        public void OnEndTurn()
        {
            StartCoroutine(OnEndTurn_Cn());

            //Local
            IEnumerator OnEndTurn_Cn()
            {
                int countBuffs = 0, balance = 0;
                ReturnSignal returnSignal;

                foreach (var demon in Actors)
                {
                    if (!demon.IsInCombat())
                    {
                        if (returnSignal = demon.IsMainProfit)
                        {
                            balance++;
                            yield return returnSignal.signal;
                        }
                        if (returnSignal = demon.IsAdvProfit)
                        {
                            countBuffs++;
                            yield return returnSignal.signal;
                        }
                    }

                    yield return s_delayHalfSecond.Restart();

                    demon.StatesUpdate();
                }

                GameContainer.Chaos.ForDemonCurse(balance);
                _artefact.Next(countBuffs);

                GameContainer.GameLoop.StartTurn();
            }
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            int progress = _parameters.cursePerTurn + s_shrinesCount * _parameters.cursePerShrine;
            if (hexId > HEX.GATE)
                hexId = (HEX.GATE << 1) - hexId;

            _curse += progress * hexId / HEX.GATE << hexId / HEX.GATE;

            if (_curse >= _maxCurse)
                LevelUp();

            _eventChanged.Invoke(this);
        }

        public void OnStartTurn()
        {
            foreach (var demon in Actors)
                demon.EffectsUpdate(_parameters.gateDefense);
        }

        public void OnPlay()
        {
            GameContainer.GameLoop.EndTurn();
        }
    }
}
