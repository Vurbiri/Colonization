//Assets\Colonization\Scripts\GameLoop\TurnQueue.cs
using System.Collections.Generic;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class TurnQueue : AReactiveValue<ITurn>, ITurn
    {
        private Id<PlayerId> _previousId = PlayerId.None, _currentId = PlayerId.Player;
        private int _turn = 1;

        public int Turn => _turn;
        public Id<PlayerId> PreviousId => _previousId;
        public Id<PlayerId> CurrentId => _currentId;
        
        public override ITurn Value { get => this; protected set { } }

        private TurnQueue() { }
        private TurnQueue(IReadOnlyList<int> data)
        {
            Errors.CheckArraySize(data, SIZE_ARRAY);

            int i = 0;
            _previousId = data[i++]; _currentId = data[i++]; _turn = data[i];
        }

        public static TurnQueue Create(ProjectSaveData saveData)
        {
            bool isLoad = saveData.TryGetTurnQueueData(out int[] data);
            TurnQueue turn = isLoad ? new(data) : new();
            saveData.TurnStateBind(turn, !isLoad);

            return turn;
        }

        public void Next()
        {
            _previousId = _currentId;
            _currentId.Next();
            if (_currentId == PlayerId.Player)
                _turn++;

            _subscriber.Invoke(this);
        }

        #region ToArray
        private const int SIZE_ARRAY = 3;
        public int[] ToArray() => new int[] { _previousId.Value, _currentId.Value, _turn };
        #endregion
    }
}
