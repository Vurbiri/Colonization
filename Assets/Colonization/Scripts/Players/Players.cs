﻿//Assets\Colonization\Scripts\Players\Players.cs
namespace Vurbiri.Colonization
{
    using Actors;
    using Characteristics;
    using Collections;
    using Data;
    using System;
    using UnityEngine;
    using static PlayerId;

    public class Players : IDisposable
    {
        private Player _current;
        private readonly IdHashSet<PlayerId, Player> _players = new();
        private readonly PlayersData _playersData;

        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        public Players(Settings settings, bool isLoading)
        {
            _playersData = new(isLoading, out bool[] loads);

            _players.Add(new Player(0, loads[0], _playersData[0], settings));

            for (int i = PlayerId.AI_01; i < PlayersCount; i++)
                _players.Add(new PlayerAI(i, loads[i], _playersData[i], settings));

            _current = _players[0];

            _playersData.Save(true);
        }

        public void Save(bool saveToFile = true, Action<bool> callback = null) => _playersData.Save(saveToFile, callback);

        public void Next() => _current = _players.Next(_current.Id.Value);

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            foreach (Player player in _players)
                player.Profit(hexId, freeGroundRes);
        }

        public void Dispose()
        {
            _playersData.Dispose();
            
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
            public RoadsFactory roadsFactory;

            public void Dispose()
            {
                Resources.UnloadAsset(states);
                states = null;
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
                if (roadsFactory.prefab == null)
                    roadsFactory.prefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();
            }
#endif
        }
        #endregion
    }
}
