//Assets\Colonization\Scripts\Diplomacy\Diplomacy.cs
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Diplomacy : IReactive<IReadOnlyList<int>>
	{
        private readonly int[] _values = new int[PlayerId.HumansCount];
        private readonly DiplomacySettings _settings;

        private readonly Subscriber<IReadOnlyList<int>> _subscriber = new();

        private int this[Id<PlayerId> idA, Id<PlayerId> idB]
        {
            get => _values[GetIndex(idA, idB)];
            set => this[GetIndex(idA, idB)] = value;
        }
        private int this[int index]
        {
            set
            {
                value = Math.Clamp(value, _settings.min, _settings.max);
                if (_values[index] == value)
                    return;

                _values[index] = value;
            }
        }

        #region Constructors
        private Diplomacy(DiplomacySettings settings, ITurn turn) 
        {
            _settings = settings;
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _values[i] = _settings.defaultValue;

            turn.Subscribe(OnNextTurn, false);
        }
        private Diplomacy(IReadOnlyList<int> values, DiplomacySettings settings, ITurn turn)
        {
            _settings = settings;
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _values[i] = values[i];

            turn.Subscribe(OnNextTurn, false);
        }

        public static Diplomacy Create(GameplaySaveData saveData, DiplomacySettings settings, ITurn turn)
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
			
			if(idA == PlayerId.Satan | idB == PlayerId.Satan)
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

            if (idA == PlayerId.Satan | idB == PlayerId.Satan)
                return !isFriendly;

            int value = this[idA, idB];
            if (value <= 0)
                return !isFriendly;

            return isFriendly = true;
        }

        public void ActorsInteraction(Id<PlayerId> idA, Id<PlayerId> idB, Relation targetAttack)
        {
            if (idA == idB | idA == PlayerId.None | idB == PlayerId.None | idA == PlayerId.Satan | idB == PlayerId.Satan | targetAttack == Relation.None)
                return;

            int index = GetIndex(idA, idB);
            int value = _values[index];

            if (targetAttack == Relation.Enemy)
                this[index] = value + (value <= 0 ? _settings.penaltyForFireOnEnemy : _settings.penaltyForFriendlyFire);
            else
                this[index] = value + _settings.rewardForBuff;

            _subscriber.Invoke(_values);
        }

        public Unsubscriber Subscribe(Action<IReadOnlyList<int>> action, bool calling = true) => _subscriber.Add(action, calling, _values);

        public void Dispose() => _subscriber.Dispose();

        private void OnNextTurn(ITurn turn)
        {
            int current = turn.CurrentId.Value;

            if (current == PlayerId.Player | current == PlayerId.Satan)
                return;

            this[current - 1] = _values[current - 1] + _settings.penaltyPerRound;
            this[PlayerId.AI_01, PlayerId.AI_02] += UnityEngine.Random.Range(_settings.penaltyPerRound, 1 - _settings.penaltyPerRound);

            _subscriber.Invoke(_values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetIndex(Id<PlayerId> idA, Id<PlayerId> idB) => idA + idB - 1;
    }
}
