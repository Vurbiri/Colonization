using System;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public abstract class GameEvents
    {
        protected readonly IdArray<GameModeId, VAction<TurnQueue, int>> _changingGameModes = new(() => new());

        public Subscription Subscribe(Id<GameModeId> gameMode, Action<TurnQueue, int> onChanging) => _changingGameModes[gameMode].Add(onChanging);
        public void Unsubscribe(Id<GameModeId> gameMode, Action<TurnQueue, int> onChanging) => _changingGameModes[gameMode].Remove(onChanging);
    }
}
