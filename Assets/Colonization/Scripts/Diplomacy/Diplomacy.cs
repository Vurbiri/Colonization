//Assets\Colonization\Scripts\Diplomacy\Diplomacy.cs
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Diplomacy : IReactive<IReadOnlyList<int>>
	{
        private const int MIN = -100, MAX = 101;

        private readonly int[] _values = new int[PlayerId.PlayersCount];
        private readonly DiplomacySettings _stt;

        private Subscriber<IReadOnlyList<int>> _subscriber;

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
            }
        }

        #region Constructors
        private Diplomacy(DiplomacySettings settings, ITurn turn) 
        {
            _stt = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _values[i] = _stt.defaultValue;

            turn.Subscribe(OnNextTurn, false);
        }
        private Diplomacy(IReadOnlyList<int> values, DiplomacySettings settings, ITurn turn)
        {
            _stt = settings;
            for (int i = 0; i < PlayerId.PlayersCount; i++)
                _values[i] = values[i];

            turn.Subscribe(OnNextTurn, false);
        }

        public static Diplomacy Create(ProjectSaveData saveData, DiplomacySettings settings, ITurn turn)
        {
            bool isLoad = saveData.TryGetDiplomacyData(out int[] data);
            Diplomacy diplomacy = isLoad ? new Diplomacy(data, settings, turn) : new Diplomacy(settings, turn);
            saveData.DiplomacyBind(diplomacy, !isLoad);

            return diplomacy;
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

            _subscriber.Invoke(_values);
        }

        public Unsubscriber Subscribe(Action<IReadOnlyList<int>> action, bool calling = true)
        {
            if (calling) action(_values);
            return _subscriber.Add(action);
        }

        private void OnNextTurn(ITurn turn)
        {
            int current = turn.CurrentId.Value;

            if (current == PlayerId.Player | current == PlayerId.Demons)
                return;

            this[current - 1] = _values[current - 1] + _stt.penaltyPerRound;
            this[PlayerId.AI_01, PlayerId.AI_02] += UnityEngine.Random.Range(_stt.penaltyPerRound, 1 - _stt.penaltyPerRound);

            _subscriber.Invoke(_values);
        }

        private int GetIndex(Id<PlayerId> idA, Id<PlayerId> idB) => idA + idB - 1;
    }
}
