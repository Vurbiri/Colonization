using System;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly IPlayerController[] _players = new IPlayerController[PlayerId.Count];

        public Human Person { get; }
        public Satan Satan { get; }

        public Players(Player.Settings settings, GameLoop game)
        {
            PersonController playerController = new(settings);
            _players[PlayerId.Person] = playerController; Person = playerController;

            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                _players[i] = new AIController(i, settings);

            SatanController satanController = new(settings);
            _players[PlayerId.Satan] = satanController;  Satan = satanController;

            game.Subscribe(GameModeId.Landing,    (turn, _) => _players[turn.currentId.Value].OnLanding());
            game.Subscribe(GameModeId.EndLanding, (turn, _) => _players[turn.currentId.Value].OnEndLanding());
            game.Subscribe(GameModeId.EndTurn,    (turn, _) => _players[turn.currentId.Value].OnEndTurn());
            game.Subscribe(GameModeId.StartTurn,  (turn, _) => _players[turn.currentId.Value].OnStartTurn());
            game.Subscribe(GameModeId.Profit,     OnProfit);
            game.Subscribe(GameModeId.Play,       (turn, _) => _players[turn.currentId.Value].OnPlay());

            settings.triggerBus.EventActorKill.Add((killer, deadType, deadId) => _players[killer].ActorKill(deadType, deadId));

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
