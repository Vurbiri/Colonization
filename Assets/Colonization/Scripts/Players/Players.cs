//Assets\Colonization\Scripts\Players\Players.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.EntryPoint;
using static Vurbiri.Colonization.PlayerId;

namespace Vurbiri.Colonization
{
    public class Players : IDisposable
    {
        private readonly Player _player;
        private readonly PlayerAI[] _playersAI = new PlayerAI[CountAI];

        private readonly TurnQueue _turnQueue;
        private readonly GameplayEventBus _eventBus;

        public Player Player => _player;
        public ITurn Turn => _turnQueue;

        #region Constructor
        public Players(SceneContainers containers, Settings settings, bool isLoading)
        {
            _eventBus = containers.Services.Get<GameplayEventBus>();

            PlayersSaveData playersData = new(isLoading);
            containers.Data.AddInstance(playersData);

            _turnQueue = CreateTurnQueue(playersData, isLoading, out Id<PlayerId> currentPlayerId);
            CreateDiplomacy(playersData, settings.diplomacy, _eventBus, containers);

            _player = new Player(0, currentPlayerId, playersData[0], settings);
            for (int i = 0; i < CountAI; i++)
                _playersAI[i] = new(i, currentPlayerId, playersData[i], settings);

            playersData.Save(true);

            #region Local: CreateDiplomacy(..)
            //=================================
            TurnQueue CreateTurnQueue(PlayersSaveData playersData, bool isLoad, out Id<PlayerId> currentPlayerId)
            {
                TurnQueue turn;

                if (isLoad && playersData.TryLoadTurnQueue(out int[] queue, out int[] data))
                    turn = new(queue, data);
                else
                    playersData.SaveTurnQueue(turn = new TurnQueue());

                currentPlayerId = turn.CurrentId;

                playersData.TurnQueueBind(turn);
                return turn;
            }
            //=================================
            void CreateDiplomacy(PlayersSaveData playersData, DiplomacySettings settings, GameplayEventBus eventBus, SceneContainers containers)
            {
                bool isLoad = playersData.DiplomacyData != null;
                Diplomacy diplomacy = isLoad ? new Diplomacy(playersData.DiplomacyData, settings, _eventBus)
                                             : new Diplomacy(settings, _eventBus);
                playersData.DiplomacyBind(diplomacy, !isLoad);
                containers.Objects.AddInstance(diplomacy);
            }
            #endregion
        }
        #endregion

        public void Next()
        {
            _turnQueue.Next();
            _eventBus.TriggerStartTurn(_turnQueue.PreviousId, _turnQueue.CurrentId);
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            _player.Profit(hexId, freeGroundRes);
            for (int i = 0; i < CountAI; i++)
                _playersAI[i].Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            _player.Dispose();
            for (int i = 0; i < CountAI; i++)
                _playersAI[i].Dispose();
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public DemonInitializer demonPrefab;
            public Transform actorsContainer;
            public PricesScriptable prices;
            public PlayerAbilitiesScriptable states;
            public DiplomacySettingsScriptable diplomacy;
            public RoadsFactory roadsFactory;

            public void Dispose()
            {
                states.Dispose();
                states = null;
                diplomacy.Dispose();
                diplomacy = null;
            }

#if UNITY_EDITOR
            public void OnValidate()
            {
                if (warriorPrefab == null)
                    warriorPrefab = VurbiriEditor.Utility.FindAnyPrefab<WarriorInitializer>();
                if (demonPrefab == null)
                    demonPrefab = VurbiriEditor.Utility.FindAnyPrefab<DemonInitializer>();
                if (prices == null)
                    prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();
                if (states == null)
                    states = VurbiriEditor.Utility.FindAnyScriptable<PlayerAbilitiesScriptable>();
                if (diplomacy == null)
                    diplomacy = VurbiriEditor.Utility.FindAnyScriptable<DiplomacySettingsScriptable>();
                if (roadsFactory.prefab == null)
                    roadsFactory.prefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();
            }
#endif
        }
        #endregion
    }
}
