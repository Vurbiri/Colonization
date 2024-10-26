using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using Data;
    using static PlayerId;

    public class Players
    {
        private Player _current;
        private readonly IdHashSet<PlayerId, Player> _players = new();
        private readonly PlayersData _playersData;

        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        public Players(Settings stt, bool isLoading)
        {
            _playersData = new(isLoading);

            PlayerObjects playerObjects;
            Roads roads;
            Player player;
            for (int i = 0; i < CountPlayers; i++)
            {
                roads = stt.roadsFactory.Create().Init(i, stt.visual[i].color);
                if (_playersData[i].IsLoad)
                    playerObjects = new(i, stt.prices, _playersData[i], stt.crossroads, roads);
                else
                    playerObjects = new(stt.prices, _playersData[i], roads);

                player = new(i, stt.visual[i], stt.states.GetAbilities(), playerObjects);
                _players.Add(player);
            }

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

        #region Nested: Settings
        //***********************************
        [Serializable]
        public class Settings : IDisposable
        {
            public PricesScriptable prices;
            public PlayerStatesScriptable states;
            public PlayersVisual visual;
            public Crossroads crossroads;
            [Space]
            public RoadsFactory roadsFactory;

            public void Dispose()
            {
                states = null;
                Resources.UnloadAsset(states);
            }
        }
        #endregion
    }
}
