using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Diplomacy : IReactive<Diplomacy>
    {
        private readonly int[] _values;
        private readonly DiplomacySettings _settings;

        private readonly Subscription<Diplomacy> _eventChanged = new();

        public int this[Id<PlayerId> idA, Id<PlayerId> idB]
        {
            [Impl(256)] get => _values[GetIndex(idA, idB)];
            [Impl(256)] set => this[GetIndex(idA, idB)] = value;
        }
        public int this[int idA, int idB]
        {
            [Impl(256)] get => _values[GetIndex(idA, idB)];
            [Impl(256)] set => this[GetIndex(idA, idB)] = value;
        }
        private int this[int index]
        {
            [Impl(256)] set
            {
                value = Math.Clamp(value, _settings.min, _settings.max);
                if (_values[index] != value)
                    _values[index] = value;
            }
        }

        public int Min { [Impl(256)] get => _settings.min - 1; }
        public int Max { [Impl(256)] get => _settings.max; }

        private Diplomacy(int[] values)
        {
            _settings = SettingsFile.Load<DiplomacySettings>();
            _values = values;
        }
        private Diplomacy() : this(new int[PlayerId.HumansCount])
        {
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _values[i] = _settings.defaultValue + i * 5;
        }

        public static Diplomacy Create(GameStorage storage, GameEvents gameEvents)
        {
            if (!storage.TryGetDiplomacy(out Diplomacy instance))
                instance = new();

            storage.BindDiplomacy(instance);
            gameEvents.Subscribe(GameModeId.Play, instance.OnGamePlay);
            return instance;
        }

        public int GetPersonRelation(int id) => _values[id - 1];

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

        public void Gift(int idA, int idB)
        {
            if (Validate(idA, idB))
            {
                int index = GetIndex(idA, idB);
                this[index] = _values[index] + _settings.penaltyForMarauding;

                _eventChanged.Invoke(this);
            }
        }

        public void Marauding(int idA, int idB)
        {
            if (Validate(idA, idB))
            {
                int index = GetIndex(idA, idB);
                this[index] = _values[index] + _settings.penaltyForMarauding;

                _eventChanged.Invoke(this);
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

                _eventChanged.Invoke(this);
            }
        }

        public Unsubscription Subscribe(Action<Diplomacy> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);

        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            int currentId = turnQueue.currentId.Value;
            if (currentId != PlayerId.Person & currentId != PlayerId.Satan)
            {
                this[currentId - 1] = _values[currentId - 1] + _settings.penaltyPerRound;
                this[PlayerId.AI_01, PlayerId.AI_02] += UnityEngine.Random.Range(_settings.penaltyPerRound, 1 - _settings.penaltyPerRound);

                _eventChanged.Invoke(this);
            }
        }

        [Impl(256)] private int GetIndex(int idA, int idB) => idA + idB - 1;

        [Impl(256)] private bool Validate(int idA, int idB) => idA != idB & idA > PlayerId.None & idB > PlayerId.None & idA < PlayerId.Satan & idB < PlayerId.Satan;
    }
}
