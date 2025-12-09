using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	sealed public partial class SatanController : Satan,  IPlayerController
	{
        private static readonly SatanControllerSettings s_settings;

        private readonly Commander _commander;

        public bool CanEnterToGate { [Impl(256)] get => _spawner.Potential == 0; }

        static SatanController() => s_settings = SettingsFile.Load<SatanControllerSettings>();
        public SatanController(Settings settings) : base(settings)
        {
            _commander = new(Actors, _spawner);
        }

        public void ActorKill(Id<ActorTypeId> type, int id)
        {
            if (type == ActorTypeId.Warrior)
                AddCurse(s_parameters.cursePerKillWarrior[id]);
        }

        public void OnLanding()
        {
            GameContainer.GameLoop.EndTurn();
        }
        public void OnEndLanding() { }

        public void OnStartTurn()
        {
            foreach (var demon in Actors)
                demon.EffectsUpdate(demon.Hexagon.IsGate ? s_parameters.gateDefense : 0);
        }

        public void OnPlay()
        {
            _coroutine = StartCoroutine(OnPlay_Cn());

            IEnumerator OnPlay_Cn()
            {
#if TEST_AI
                Log.Info("====================== Satan ======================");
#endif

                yield return s_settings.waitPlayStart.Restart();
                yield return _waitAll.Add(s_settings.waitPlay.Restart(), _commander.Execution_Cn());

#if TEST_AI
                Log.Info("===================================================");
#endif

                GameContainer.GameLoop.EndTurn();
                _coroutine = null;
            }
        }

        public void OnEndTurn()
        {
            _coroutine = StartCoroutine(OnEndTurn_Cn());

            // ======= Local ==========
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
                _coroutine = null;
            }
        }

        public void OnProfit(Id<PlayerId> id, int hexId)
        {
            int progress = s_parameters.cursePerTurn + s_shrinesCount * s_parameters.cursePerShrine;
            if (hexId > HEX.GATE)
                hexId = (HEX.GATE << 1) - hexId;

            AddCurse(progress * hexId / HEX.GATE << hexId / HEX.GATE);
        }
    }
}
