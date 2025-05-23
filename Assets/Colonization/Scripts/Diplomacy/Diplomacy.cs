//Assets\Colonization\Scripts\Diplomacy\Diplomacy.cs
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Diplomacy : IReactive<IReadOnlyList<int>>
	{
        private readonly int[] _values;
        private readonly DiplomacySettings _settings;

        private readonly Subscription<IReadOnlyList<int>> _eventChanged = new();

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
        private Diplomacy() : this(new int[PlayerId.HumansCount])
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _values[i] = _settings.defaultValue;
        }
        private Diplomacy(int[] values)
        {
            _settings = SettingsFile.Load<DiplomacySettings>();
            _values = values;
        }

        public static Diplomacy Create(GameplayStorage storage, GameEvents game)
        {
            Diplomacy diplomacy = storage.TryGetDiplomacyData(out int[] data) ? new(data) : new();
            storage.DiplomacyBind(diplomacy);

            game.Subscribe(GameModeId.Play, diplomacy.OnGamePlay);

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

            _eventChanged.Invoke(_values);
        }

        public Unsubscription Subscribe(Action<IReadOnlyList<int>> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values);

        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            int currentId = turnQueue.currentId.Value;

            if (currentId == PlayerId.Player | currentId == PlayerId.Satan)
                return;

            this[currentId - 1] = _values[currentId - 1] + _settings.penaltyPerRound;
            this[PlayerId.AI_01, PlayerId.AI_02] += UnityEngine.Random.Range(_settings.penaltyPerRound, 1 - _settings.penaltyPerRound);

            _eventChanged.Invoke(_values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetIndex(Id<PlayerId> idA, Id<PlayerId> idB) => idA + idB - 1;
    }
}
