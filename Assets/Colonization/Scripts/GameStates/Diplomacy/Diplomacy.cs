using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class Diplomacy : IReactive<int[]>
    {
        private readonly int[] _values;
        private readonly DiplomacySettings _settings;

        private readonly Subscription<int[]> _eventChanged = new();

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
                if (_values[index] != value)
                    _values[index] = value;
            }
        }

        public Diplomacy(GameStorage storage, GameEvents gameEvents)
        {
            _settings = SettingsFile.Load<DiplomacySettings>();

            if (!storage.TryGetDiplomacyData(out _values))
            {
                _values = new int[PlayerId.HumansCount];
                for (int i = 0; i < PlayerId.HumansCount; i++)
                    _values[i] = _settings.defaultValue;
            }
            storage.BindDiplomacy(this);

            gameEvents.Subscribe(GameModeId.Play, OnGamePlay);
        }

        public Relation GetRelation(Id<PlayerId> idA, Id<PlayerId> idB)
        {
            if (idA == PlayerId.None | idB == PlayerId.None)
                return Relation.None;

            if (idA == idB)
                return Relation.Friend;

            if (idA == PlayerId.Satan | idB == PlayerId.Satan)
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

        public void Marauding(int idA, int idB)
        {
            if (Validate(idA, idB))
            {
                int index = GetIndex(idA, idB);
                this[index] = _values[index] + _settings.penaltyForMarauding;

                _eventChanged.Invoke(_values);
            }
        }

        public void ActorsInteraction(int idA, int idB, Relation targetAttack)
        {
            if (Validate(idA, idB) & targetAttack != Relation.None)
            {
                int index = GetIndex(idA, idB);
                int value = _values[index];

                if (targetAttack == Relation.Enemy)
                    this[index] = value + (value <= 0 ? _settings.penaltyForFireOnEnemy : _settings.penaltyForFriendlyFire);
                else
                    this[index] = value + _settings.rewardForBuff;

                _eventChanged.Invoke(_values);
            }
        }

        public Unsubscription Subscribe(Action<int[]> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values);

        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            int currentId = turnQueue.currentId.Value;
            if (currentId != PlayerId.Person & currentId != PlayerId.Satan)
            {
                this[currentId - 1] = _values[currentId - 1] + _settings.penaltyPerRound;
                this[PlayerId.AI_01, PlayerId.AI_02] += UnityEngine.Random.Range(_settings.penaltyPerRound, 1 - _settings.penaltyPerRound);

                _eventChanged.Invoke(_values);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetIndex(int idA, int idB) => idA + idB - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Validate(int idA, int idB) => idA != idB & idA > PlayerId.None & idB > PlayerId.None & idA < PlayerId.Satan & idB < PlayerId.Satan;
    }
}
