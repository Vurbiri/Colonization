using System;
using Vurbiri.Colonization.EntryPoint;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly IPlayerController[] _players = new IPlayerController[PlayerId.Count];

        public Players(Player.Settings settings, GameContent content)
        {
            _players[PlayerId.Person] = content.person = new(settings);

            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                _players[i] = content.ai[i-1] =new AIController(i, settings);

            _players[PlayerId.Satan] = content.satan = new SatanController(settings);

            GameLoop game = content.gameLoop;
            game.Subscribe(GameModeId.Landing,    (turn, _) => _players[turn.currentId.Value].OnLanding());
            game.Subscribe(GameModeId.EndLanding, (turn, _) => _players[turn.currentId.Value].OnEndLanding());
            game.Subscribe(GameModeId.EndTurn,    (turn, _) => _players[turn.currentId.Value].OnEndTurn());
            game.Subscribe(GameModeId.StartTurn,  (turn, _) => _players[turn.currentId.Value].OnStartTurn());
            game.Subscribe(GameModeId.Profit,     OnProfit);
            game.Subscribe(GameModeId.Play,       (turn, _) => _players[turn.currentId.Value].OnPlay());

            GameContainer.EventBus.EventActorKill.Add((killer, deadType, deadId) => _players[killer].ActorKill(deadType, deadId));

            Player.Init();

            settings.Dispose();
        }

        public void Dispose()
        {
            Player.Clear();

            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].Dispose();
        }

        private void OnProfit(TurnQueue turnQueue, int hexId)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].OnProfit(turnQueue.currentId, hexId);
        }
    }
}
