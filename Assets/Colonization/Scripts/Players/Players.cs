using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    using static CONST;

    public class Players : ASingleton<Players>, IEnumerable<Player>
    {
        [Space]
        [SerializeField] private CurrenciesLite _startResources;
        [Space]
        [SerializeField] private PlayerStatesScriptable _states;
        [SerializeField] private PlayerVisualSetScriptable _visualSet;

        private Player _current;
        private readonly IdHashSet<PlayerId, Player> _players = new();
        
        public Player Current => _current;
        public Player this[Id<PlayerId> id] => _players[id];

        public void StartGame(int[] idVisuals)
        {
            int idVisual;
            Player player;
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                idVisual = idVisuals[i];
                player = new(i, _visualSet.Get(idVisual), new(_startResources), _states);
                _players.Replace(player);
                StartCoroutine(player.Save_Coroutine(i == MAX_PLAYERS - 1));
            }

            _current = _players[0];
        }

        public void LoadGame(int[] idVisuals, Crossroads crossroads)
        {
            int idVisual;
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                idVisual = idVisuals[i];
                _players.Replace(new(i, _visualSet.Get(idVisual), _states));
            }

            foreach (var player in _players)
                player.Load(crossroads);

            _current = _players[0];
        }

        public void Next() => _current = _players.Next(_current.Id.ToInt);

        public void Profit(int hexId, ACurrencies freeGroundRes)
        {
            foreach (Player player in _players)
                player.Profit(hexId, freeGroundRes);
        }

        public void DestroyGame()
        {

        }

        public IEnumerator<Player> GetEnumerator() => _players.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _players.GetEnumerator();
    }
}
