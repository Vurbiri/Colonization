//Assets\Colonization\Scripts\Players\Players.cs
namespace Vurbiri.Colonization
{
    using Actors;
    using Characteristics;
    using Collections;
    using Data;
    using System;
    using UnityEngine;
    using Vurbiri.EntryPoint;
    using static PlayerId;

    public class Players : IDisposable
    {
        private readonly IdArray<PlayerId, Player> _players = new();
        private readonly GameplayEventBus _eventBus;
        private Player _current;

        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        #region Constructor
        public Players(SceneContainers containers, Settings settings, bool isLoading)
        {
            _eventBus = containers.Services.Get<GameplayEventBus>();

            PlayersData playersData = new(isLoading, out bool[] loads, out bool isLoadDiplomacy);
            containers.Data.AddInstance(playersData);

            Diplomacy diplomacy = isLoadDiplomacy ? new Diplomacy(playersData.DiplomacyData, settings.diplomacy, _eventBus) 
                                                  : new Diplomacy(settings.diplomacy, _eventBus);
            playersData.DiplomacyBind(diplomacy, !isLoadDiplomacy);
            containers.Objects.AddInstance(diplomacy);

            _players[0] = new Player(0, loads[0], playersData[0], settings);

            for (int i = PlayerId.AI_01; i < PlayersCount; i++)
                _players[i] = new PlayerAI(i, loads[i], playersData[i], settings);

            _current = _players[0];

            playersData.Save(true);
        }
        #endregion

        public void Next()
        {
            Id<PlayerId> prev = _current.Id; 
            _current = _players.Next(_current.Id.Value); ;
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
            public Transform warriorsContainer;
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
