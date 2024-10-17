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
        private Roads[] _roads = new Roads[MAX_PLAYERS];

        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        public Players(PlayerStatesScriptable states, PlayerVisualSetScriptable visualSet, int[] visualIds, RoadsFactory roadsFactory)
        {
            Player player;
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                player = new(i, visualSet.Get(visualIds[i]), states.GetAbilities(), _roads[i] = roadsFactory.Create());
                _players.Add(player);
            }

            _current = _players[0];
        }

        public void SetData(IReadOnlyDIContainer container, PricesScriptable prices, Crossroads crossroads, bool isLoading)
        {
            if (isLoading)
                _playersData = new(container, crossroads, _roads);
            else
                _playersData = new(container, prices, _roads);

            for (int i = 0; i < MAX_PLAYERS; i++)
                _players[i].SetData(_playersData[i]);

            _roads = null;

            _playersData.Save(true);
        }

        public void Save(bool saveToFile = true, Action<bool> callback = null) => _playersData.Save(saveToFile, callback);

        public void Next() => _current = _players.Next(_current.Id.ToInt);

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            foreach (Player player in _players)
                player.Profit(hexId, freeGroundRes);
        }
    }
}
