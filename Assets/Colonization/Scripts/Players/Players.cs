using System;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly Array<HumanController> _humans = new(PlayerId.HumansCount);
        private readonly SatanController _satan;

        public IPlayerController this[int id] { [Impl(256)] get => id < PlayerId.HumansCount ? _humans[id] : _satan; }
        public HumanController Person { [Impl(256)] get => _humans[PlayerId.Person]; }
        public ReadOnlyArray<HumanController> Humans { [Impl(256)] get => _humans;  }
        public SatanController Satan { [Impl(256)] get => _satan; }

        public Players(Player.Settings settings, GameLoop game)
        {

#if TEST_AI
            UnityEngine.Debug.LogWarning("[Players] TEST_AI");
            _humans[PlayerId.Person] = new AIController(PlayerId.Person, settings);
#else
            _humans[PlayerId.Person] = new PersonController(settings);
#endif
            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                _humans[i] = new AIController(i, settings);

            _satan = new(settings);

            game.Subscribe(GameModeId.Landing, (turn, _) => this[turn.currentId.Value].OnLanding());
            game.Subscribe(GameModeId.EndLanding, (turn, _) => this[turn.currentId.Value].OnEndLanding());
            game.Subscribe(GameModeId.EndTurn, (turn, _) => this[turn.currentId.Value].OnEndTurn());
            game.Subscribe(GameModeId.StartTurn, (turn, _) => this[turn.currentId.Value].OnStartTurn());
            game.Subscribe(GameModeId.Profit, OnProfit);
            game.Subscribe(GameModeId.Play, (turn, _) => this[turn.currentId.Value].OnPlay());

            GameContainer.EventBus.EventActorKill.Add((killer, deadType, deadId) => this[killer].ActorKill(deadType, deadId));

            settings.Dispose();
        }

        private void OnProfit(TurnQueue turnQueue, int hexId)
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i].OnProfit(turnQueue.currentId, hexId);
            _satan.OnProfit(turnQueue.currentId, hexId);

            GameContainer.GameLoop.Play();
        }

        public void Dispose()
        {
            Player.Reset();

            for (int i = 0; i < PlayerId.HumansCount; i++)
                _humans[i].Dispose();
            _satan.Dispose();
        }
    }
}
