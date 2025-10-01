using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization
{
	sealed public class SatanController : Satan,  IPlayerController
	{
        private int _shrinesCount;
        
        public SatanController(Settings settings, ReadOnlyArray<HumanController> humans) : base(settings)
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
                humans[i].Shrines.Subscribe((_, _, _) => _shrinesCount++);
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
            OnEndTurn_Cn().Start();

            //Local
            IEnumerator OnEndTurn_Cn()
            {
                int countBuffs = 0, balance = 0;
                ReturnSignal returnSignal;

                foreach (var demon in Actors)
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

                    demon.StatesUpdate();
                }

                GameContainer.Chaos.ForDemonCurse(balance);
                _artefact.Next(countBuffs);

                GameContainer.GameLoop.StartTurn();
            }
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            int progress = _parameters.cursePerTurn + _shrinesCount * _parameters.cursePerShrine;
            if (hexId > CONST.GATE_ID)
                hexId = (CONST.GATE_ID << 1) - hexId;

            _curse += progress * hexId / CONST.GATE_ID << hexId / CONST.GATE_ID;

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
