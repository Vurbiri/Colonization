using System;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class GameEvents
    {
        protected readonly ReadOnlyIdArray<GameModeId, VAction<TurnQueue, int>> _changeGameModes = new(() => new());
        protected readonly VAction<Id<GameModeId>, TurnQueue> _changeGameMode = new();

        protected Id<GameModeId> _gameMode;
        protected TurnQueue _turnQueue;

        public Id<GameModeId> GameMode { [Impl(256)] get => _gameMode; }
        public TurnQueue TurnQueue { [Impl(256)] get => _turnQueue; }
        public Id<PlayerId> CurrentPlayer { [Impl(256)] get => _turnQueue.currentId; }

        public Subscription Subscribe(Id<GameModeId> gameMode, Action<TurnQueue, int> onChanging) => _changeGameModes[gameMode].Add(onChanging);
        public void Unsubscribe(Id<GameModeId> gameMode, Action<TurnQueue, int> onChanging) => _changeGameModes[gameMode].Remove(onChanging);

        public Subscription Subscribe(Action<Id<GameModeId>, TurnQueue> onChanging) => _changeGameMode.Add(onChanging, _gameMode, _turnQueue);
        public void Unsubscribe(Action<Id<GameModeId>, TurnQueue> onChanging) => _changeGameMode.Remove(onChanging);
    }
}
