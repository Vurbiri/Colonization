using System;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Players
    {
        private Player _current;
        private readonly IdHashSet<PlayerId, Player> _players = new();
        private PlayersData _playersData;

        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        public Players(PlayerStatesScriptable states, PlayerVisualSetScriptable visualSet, int[] visualIds)
        {
            Player player;
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                player = new(i, visualSet.Get(visualIds[i]), states.GetAbilities());
                _players.Add(player);
            }

            _current = _players[0];
        }

        public void Setup(IReadOnlyDIContainer services, PricesScriptable prices, Crossroads crossroads, RoadsFactory roadsFactory, bool isLoading)
        {
            Roads[] roads = new Roads[MAX_PLAYERS];
            for (int i = 0; i < MAX_PLAYERS; i++)
                roads[i] = roadsFactory.Create().Init(i, _players[i].Color);

            _playersData = new(services, prices, roads, crossroads, isLoading);

            for (int i = 0; i < MAX_PLAYERS; i++)
                _players[i].SetData(_playersData[i]);

            _playersData.Save(true);
        }

        public void Save(bool saveToFile = true, Action<bool> callback = null) => _playersData.Save(saveToFile, callback);

        public void Next() => _current = _players.Next(_current.Id.Value);

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            foreach (Player player in _players)
                player.Profit(hexId, freeGroundRes);
        }
    }
}
