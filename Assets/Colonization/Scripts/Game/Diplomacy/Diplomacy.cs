using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Diplomacy : IReactive<Diplomacy>
    {
        private const int AI_INDEX = PlayerId.AI_01 + PlayerId.AI_02 - 1;

        private readonly int[] _values;
        private readonly DiplomacySettings _settings;

        private readonly VAction<Diplomacy> _eventChanged = new();

        public int this[Id<PlayerId> idA, Id<PlayerId> idB] { [Impl(256)] get => _values[GetIndex(idA, idB)]; }
        public int this[int idA, int idB] { [Impl(256)] get => _values[GetIndex(idA, idB)]; }
        private int this[int index] { [Impl(256)] set => _values[index] = Math.Clamp(value, _settings.min, _settings.max); }

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
                _values[i] = _settings.defaultValue + i * _settings.defaultDelta;
        }

        public static Diplomacy Create(GameStorage storage, GameEvents gameEvents)
        {
            if (!storage.TryGetDiplomacy(out Diplomacy instance))
                instance = new();

            storage.BindDiplomacy(instance);
            gameEvents.Subscribe(GameModeId.Play, instance.OnGamePlay);
            return instance;
        }

        [Impl(256)] public int GetRelationToPerson(int id) => _values[id - 1];

        [Impl(256)] public bool IsFriend(Id<PlayerId> idA, Id<PlayerId> idB) => Validate(idA, idB) && (idA == idB || (IsNotSatan(idA, idB) && this[idA, idB] > 0));
        [Impl(256)] public bool IsGreatFriend(Id<PlayerId> idA, Id<PlayerId> idB) => Validate(idA, idB) && (idA == idB || (IsNotSatan(idA, idB) && this[idA, idB] > _settings.great));
        [Impl(256)] public bool IsEnemy(Id<PlayerId> idA, Id<PlayerId> idB) => (Validate(idA, idB) & idA != idB) && (IsSatan(idA, idB) || this[idA, idB] <= 0);
        [Impl(256)] public bool IsGreatEnemy(Id<PlayerId> idA, Id<PlayerId> idB) => (Validate(idA, idB) & idA != idB) && (IsSatan(idA, idB) || this[idA, idB] <= -_settings.great);

        [Impl(256)] public void Gift(int id, int giver) => Set(id, giver, id == PlayerId.Person ? _settings.rewardForGift >> 1 : _settings.rewardForGift);
        [Impl(256)] public void Marauding(int idA, int idB) => Set(idA, idB, _settings.penaltyForMarauding);
        [Impl(256)] public void Occupation(int idA, int idB, int value) => Set(idA, idB, value);

        [Impl(256)] public Subscription Subscribe(Action<Diplomacy> action, bool instantGetValue = true) => _eventChanged.Add(action, this, instantGetValue);

        #region ================== ActorsInteraction ============================
        public bool IsCanActorsInteraction(Id<PlayerId> idA, Id<PlayerId> idB, Relation typeAction, out bool isFriendly)
        {
            bool valid = Validate(idA, idB) & typeAction != Relation.None;
            isFriendly = typeAction == Relation.Friend;
            return valid && (idA == idB ? isFriendly : IsSatan(idA, idB) ? !isFriendly : this[idA, idB] <= 0 ? !isFriendly : isFriendly = true); ;
        }

        public void ActorsInteraction(Id<PlayerId> idA, Id<PlayerId> idB, Relation targetAttack, bool isCombat)
        {
            if (idA != idB & IsNotSatan(idA, idB))
            {
                int index = GetIndex(idA, idB);
                int value = _values[index];

                if (targetAttack == Relation.Enemy)
                {
                    this[index] = value + (value <= 0 ? _settings.penaltyForFireOnEnemy : _settings.penaltyForFriendlyFire);
                    _eventChanged.Invoke(this);
                }
                else if (isCombat)
                {
                    this[index] = value + _settings.rewardForBuff;
                    _eventChanged.Invoke(this);
                }
            }
        }
        #endregion

        #region ================== Utilities ============================
        private void OnGamePlay(TurnQueue turnQueue, int dice)
        {
            int currentId = turnQueue.currentId.Value;
            if (currentId != PlayerId.Person & currentId != PlayerId.Satan)
            {
                int pnIndex = currentId - 1, pnValue = _values[pnIndex];
                int aiAdd = _settings.aiPerRound, pnAdd = _settings.personPerRound + (pnValue > _settings.great ? 1 : 0);

                this[AI_INDEX] = _values[AI_INDEX] + aiAdd;
                this[pnIndex] = pnValue + pnAdd;
                
                if(pnAdd != 0 || aiAdd != 0)
                    _eventChanged.Invoke(this);
            }
        }

        [Impl(256)] private void Set(int idA, int idB, int value)
        {
            int index = GetIndex(idA, idB);
            this[index] = _values[index] + value;

            _eventChanged.Invoke(this);
        }

        [Impl(256)] private static int GetIndex(int idA, int idB) => idA + idB - 1;

        [Impl(256)] private static bool Validate(Id<PlayerId> idA, Id<PlayerId> idB) => idA != PlayerId.None & idB != PlayerId.None;
        [Impl(256)] private static bool IsSatan(Id<PlayerId> idA, Id<PlayerId> idB) => idA == PlayerId.Satan | idB == PlayerId.Satan;
        [Impl(256)] private static bool IsNotSatan(Id<PlayerId> idA, Id<PlayerId> idB) => idA != PlayerId.Satan & idB != PlayerId.Satan;

        #endregion
    }
}
