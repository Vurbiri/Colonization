using System;
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    using static PlayerId;

    public class Players
    {
        private Player _current;
        private readonly IdHashSet<PlayerId, Player> _players = new();
        private readonly PlayersData _playersData;

        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        public Players(Settings settings, bool isLoading)
        {
            Roads[] roads = new Roads[CountPlayers];
            for (int i = 0; i < CountPlayers; i++)
                roads[i] = settings.roadsFactory.Create().Init(i, settings.prices.Road, settings.visual[i].color);

            _playersData = new(settings.prices, roads, settings.crossroads, isLoading);

            Player player;
            for (int i = 0; i < CountPlayers; i++)
            {
                player = new(i, settings.visual[i], settings.states.GetAbilities(), _playersData[i]);
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
