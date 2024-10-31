using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using Actors;
    using Data;
    using Vurbiri.Collections;
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

            for (int i = 0; i < PlayersCount; i++)
                _players.Add(new(i, new(i, loads[i], _playersData[i], settings)));

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
        }

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public WarriorInitializer warriorPrefab;
            public Transform warriorsContainer;
            public PricesScriptable prices;
            public PlayerStatesScriptable states;
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
                    states = VurbiriEditor.Utility.FindAnyScriptable<PlayerStatesScriptable>();
                if (roadsFactory.prefab == null)
                    roadsFactory.prefab = VurbiriEditor.Utility.FindAnyPrefab<Roads>();
            }
#endif
        }
        #endregion
    }
}
