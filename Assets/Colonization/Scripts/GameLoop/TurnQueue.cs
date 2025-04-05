//Assets\Colonization\Scripts\GameLoop\TurnQueue.cs
using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class TurnQueue : IReactive<TurnQueue>
    {
        private Id<PlayerId> _previousId, _currentId;
        private int _turn;
        private readonly Signer<TurnQueue> _signer = new();

        public int Turn => _turn;
        public Id<PlayerId> PreviousId => _previousId;
        public Id<PlayerId> CurrentId => _currentId;

        private TurnQueue() 
        {
            _previousId = PlayerId.None; _currentId = PlayerId.Player; _turn = 1;
        }
        public TurnQueue(int previousId, int currentId, int turn)
        {
            _previousId = previousId; _currentId = currentId; _turn = turn;
        }

        public static TurnQueue Create(GameplayStorage storage)
        {
            bool isLoad;
            if(!(isLoad = storage.TryGetTurnQueue(out TurnQueue turn))) turn = new();
            storage.TurnQueueBind(turn, !isLoad);

            return turn;
        }

        public Unsubscriber Subscribe(Action<TurnQueue> action, bool instantGetValue = true) => _signer.Add(action, instantGetValue, this);

        public void Next()
        {
            _previousId = _currentId;
            _currentId.Next();
            if (_currentId == PlayerId.Player)
                _turn++;

            _signer.Invoke(this);
        }
    }
}
