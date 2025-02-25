//Assets\Colonization\Scripts\Diplomacy\Diplomacy.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Diplomacy : IReactive<int, int>
	{
        private const int MIN = -100, MAX = 101;

        private readonly int[] _values = new int[PlayerId.PlayersCount];
        private readonly DiplomacySettings _stt;

        private readonly Subscriber<int, int> _subscriber = new();

        private int this[Id<PlayerId> idA, Id<PlayerId> idB]
        {
            get => _values[idA + idB - 1];
            set => this[idA + idB - 1] = value;
        }
        private int this[int index]
        {
            set
            {
                value = Math.Clamp(value, MIN, MAX);
                if (_values[index] == value)
                    return;

                _values[index] = value;
                _subscriber.Invoke(index, value);
            }
        }

        #region Constructors
        public Diplomacy(DiplomacySettings settings, GameplayEventBus eventBus) 
        {
            _stt = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _values[i] = _stt.defaultValue;

            eventBus.EventStartTurn += OnStartTurn;
        }
        public Diplomacy(IReadOnlyList<int> values, DiplomacySettings settings, GameplayEventBus eventBus)
        {
            _stt = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _values[i] = values[i];

            eventBus.EventStartTurn += OnStartTurn;
        }
        #endregion

        public Relation GetRelation(Id<PlayerId> idA, Id<PlayerId> idB)
		{
            if (idA == PlayerId.None | idB == PlayerId.None)
                return Relation.None;

            if (idA == idB)
				return Relation.Friend;
			
			if(idA == PlayerId.Demons | idB == PlayerId.Demons)
				return Relation.Enemy;

			return this[idA, idB] > 0 ? Relation.Friend : Relation.Enemy;
        }

        public bool IsCanActorsInteraction(Id<PlayerId> idA, Id<PlayerId> idB, Relation typeAction, out bool isFriendly)
        {
            isFriendly = typeAction == Relation.Friend;
            if (idA == PlayerId.None | idB == PlayerId.None | typeAction == Relation.None)
                return false;

            if (idA == idB)
                return isFriendly;

            if (idA == PlayerId.Demons | idB == PlayerId.Demons)
                return !isFriendly;

            int value = this[idA, idB];
            if (value <= 0)
                return !isFriendly;

            return isFriendly = true;
        }

        public void ActorsInteraction(Id<PlayerId> idA, Id<PlayerId> idB, Relation targetAttack)
        {
            if (idA == idB | idA == PlayerId.None | idB == PlayerId.None | idA == PlayerId.Demons | idB == PlayerId.Demons | targetAttack == Relation.None)
                return;

            int index = GetIndex(idA, idB);
            int value = _values[index];

            if (targetAttack == Relation.Enemy)
                this[index] = value + (value <= 0 ? _stt.penaltyForAttackingEnemy : _stt.penaltyForFriendlyFire);
            else
                this[index] = value + _stt.rewardForBuff;

        }

        #region IReactive
        public IUnsubscriber Subscribe(Action<int, int> action, bool calling = true)
        {
            if (calling)
            {
                for (int i = 0; i < PlayerId.PlayersCount; i++)
                    action(i, _values[i]);
            }

            return _subscriber.Add(action);
        }
        #endregion

        private void OnStartTurn(Id<PlayerId> prev, Id<PlayerId> current)
        {
            if (current == PlayerId.Player | current == PlayerId.Demons)
                return;

            this[current - 1] = _values[current - 1] + _stt.penaltyPerRound;
            this[PlayerId.AI_01, PlayerId.AI_02] += UnityEngine.Random.Range(_stt.penaltyPerRound, 1 - _stt.penaltyPerRound);
        }

        private int GetIndex(Id<PlayerId> idA, Id<PlayerId> idB) => idA + idB - 1;
    }
}
