namespace Vurbiri.Colonization
{ 
	public class PlayersController
	{
        private readonly IPlayerController[] _players = new IPlayerController[PlayerId.Count];

        public PlayersController(GameLoop game)
        {
            game.Subscribe(GameModeId.Landing, (turn, _)    => _players[turn.currentId.Value].OnLanding());
            game.Subscribe(GameModeId.EndLanding, (turn, _) => _players[turn.currentId.Value].OnEndLanding());
            game.Subscribe(GameModeId.EndTurn, (turn, _)    => _players[turn.currentId.Value].OnEndTurn());
            game.Subscribe(GameModeId.StartTurn, (turn, _)  => _players[turn.currentId.Value].OnStartTurn());
            game.Subscribe(GameModeId.Profit, OnProfit);
            game.Subscribe(GameModeId.Play, (turn, _)       => _players[turn.currentId.Value].OnPlay());

            GameContainer.EventBus.EventActorKill.Add((killer, deadType, deadId) => _players[killer].ActorKill(deadType, deadId));
        }

        public void Add(int id, IPlayerController player) => _players[id] = player;

        private void OnProfit(TurnQueue turnQueue, int hexId)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _players[i].OnProfit(turnQueue.currentId, hexId);

            GameContainer.GameLoop.Play();
        }
    }
}
