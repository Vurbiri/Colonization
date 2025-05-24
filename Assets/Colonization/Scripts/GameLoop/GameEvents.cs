using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public delegate void ChangingGameMode(TurnQueue turnQueue, int dice);

    public abstract class GameEvents
    {
        protected readonly IdArray<GameModeId, ChangingGameMode> _changingGameModes = new();

        public void Subscribe(Id<GameModeId> gameMode, ChangingGameMode onChanging) => _changingGameModes[gameMode] += onChanging;

    }
}
