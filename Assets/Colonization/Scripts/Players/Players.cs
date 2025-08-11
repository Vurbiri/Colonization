using System;
using System.Runtime.CompilerServices;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly IPlayerController[] _players = new IPlayerController[PlayerId.Count];

        private readonly PersonController _person;
        private readonly AIController[] _ai = new AIController[PlayerId.AICount];
        private readonly SatanController _satan;

        public PersonController Person
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _person;
        }
        public SatanController Satan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _satan;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AIController GetAI(Id<PlayerId> id) => _ai[id.Value - PlayerId.AI_01];

        public Players(Player.Settings settings, GameLoop game)
        {
            _players[PlayerId.Person] = _person = new(settings);

            for (int i = PlayerId.AI_01; i < PlayerId.HumansCount; i++)
                _players[i] = _ai[i - PlayerId.AI_01] = new(i, settings);

            _players[PlayerId.Satan] = _satan = new(settings);

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
