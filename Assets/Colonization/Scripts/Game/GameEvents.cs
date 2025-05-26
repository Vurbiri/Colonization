using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public delegate void ChangingGameMode(TurnQueue turnQueue, int dice);

    public abstract class GameEvents
    {
        protected readonly IdArray<GameModeId, ChangingGameMode> _changingGameModes = new();

        protected GameEvents() 
        {
            for (int i = 0; i < GameModeId.Count; i++)
                _changingGameModes[i] += (_, _) => { };
        }

        public void Subscribe(Id<GameModeId> gameMode, ChangingGameMode onChanging) => _changingGameModes[gameMode] += onChanging;
        public void Unsubscribe(Id<GameModeId> gameMode, ChangingGameMode onChanging) => _changingGameModes[gameMode] -= onChanging;
    }
}
