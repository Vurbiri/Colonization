//Assets\Colonization\Scripts\GameLoop\TurnQueue.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class TurnQueue : AReactive<ITurn>, ITurn, IEnumerable<int>
    {
        private readonly int[] _queue = new int[PlayerId.Count];
        private int _prevId = PlayerId.None;
        private int _currentIndex = 0;
        private int _turn = 1;

        public int Turn => _turn;
        public int PrevId => _prevId;
        public int CurrentId => _queue[_currentIndex];
        
        public override ITurn Value { get => this; protected set { } }

        public TurnQueue()
        {
            int[] array = new int[PlayerId.PlayersCount];
            Debug.Log("Раскомментить в TurnQueue");
            //array.Fill().Shuffle();
            array.Fill();

            int i = 0;
            for(; i< PlayerId.PlayersCount; i++)
                _queue[i] = array[i];
            _queue[i] = PlayerId.Demons;
        }

        public TurnQueue(IReadOnlyList<int> queue, IReadOnlyList<int> data)
        {
            if ((queue == null || queue.Count != PlayerId.Count) | (data == null || data.Count != 3))
                throw new ArgumentException($"IReadOnlyList<int> queue = {queue} | IReadOnlyList<int> turns = {data}");

            
            for (int j = 0; j < PlayerId.Count; j++)
                _queue[j] = queue[j];

            int i = 0;
            _prevId           = data[i++];
            _currentIndex     = data[i++];
            _turn             = data[i++];
        }

        public void Next()
        {
            _prevId = _queue[_currentIndex];
            _currentIndex = (_currentIndex + 1) % PlayerId.Count;
            _turn++;

            actionValueChange?.Invoke(this);
        }

        #region IArrayable
        public int[] ToArray() => new int[] { _prevId, _currentIndex, _turn };
        public void ToArray(int[] array)
        {
            if (array == null || array.Length != 3)
                throw new ArgumentException($"int[] array = {array}");

            int i = 0;
            array[i++] = _prevId;
            array[i++] = _currentIndex;
            array[i++] = _turn;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < PlayerId.Count; i++)
                yield return _queue[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion


    }
}
