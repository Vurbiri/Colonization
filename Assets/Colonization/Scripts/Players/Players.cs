//Assets\Colonization\Scripts\Players\Players.cs
namespace Vurbiri.Colonization
{
    using Actors;
    using Characteristics;
    using Collections;
    using Data;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Vurbiri.EntryPoint;
    using static PlayerId;

    public class Players : IDisposable
    {
        private readonly IdArray<PlayerId, Player> _players = new();
        private readonly Queue<Id<PlayerId>> _turnQueue = new(PlayerId.Count);
        private readonly GameplayEventBus _eventBus;
        private Player _current;

        public Player Player => _players[PlayerId.Player];

        public IPlayer Current => _current;
        public IPlayer this[Id<PlayerId> id] => _players[id];

        #region Constructor
        public Players(SceneContainers containers, Settings settings, bool isLoading)
        {
            _eventBus = containers.Services.Get<GameplayEventBus>();

            PlayersData playersData = new(isLoading, out bool[] loads, out bool isLoadDiplomacy);
            containers.Data.AddInstance(playersData);

            int currentPlayerId = playersData.LoadCurrentPlayerId(isLoading, 0);

            CreateDiplomacy(playersData, settings.diplomacy, _eventBus, containers, isLoadDiplomacy);

            _players[0] = new Player(0, currentPlayerId, loads[0], playersData[0], settings);

            for (int i = AI_01; i < PlayersCount; i++)
                _players[i] = new PlayerAI(i, currentPlayerId, loads[i], playersData[i], settings);

            _current = _players[currentPlayerId];

            playersData.Save(currentPlayerId, true);

            #region Local: CreateDiplomacy(..)
            //=================================
            //void CreateTurnQueue(PlayersData playersData, bool isLoad)
            //{

            //}
            //=================================
            void CreateDiplomacy(PlayersData playersData, DiplomacySettings settings, GameplayEventBus eventBus, SceneContainers containers, bool isLoad)
            {
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
            Id<PlayerId> prev = _current.Id; 
            _current = _players.Next(_current.Id.Value);
            _eventBus.TriggerStartTurn(prev, _current.Id);
        }

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            for (int i = 0; i < PlayersCount; i++)
                _players[i].Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            for(int i = 0; i < PlayersCount; i++)
                _players[i].Dispose();
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
