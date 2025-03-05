//Assets\Colonization\Scripts\Players\Demon\Demon.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    using static GATE;

    public class Demon : IDisposable
    {
        private int _curse;
        private int _level;
        private int _cursePerTurn;

        private readonly ReactiveCombination<int, int> _countPerkAndShrine;
        private Unsubscribers _unsubscribers = new();

        private readonly Hexagon _gateHex;
        private readonly DemonsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _demons = new();

        public Demon(Hexagon gateHex, Player[] players, Players.Settings settings)
        {
            _gateHex = gateHex;
            _spawner = new(settings.demonPrefab, settings.actorsContainer, gateHex);

            ReactiveIntUnion perksCount = new(PlayerId.PlayersCount), shrineCount = new(PlayerId.PlayersCount);
            for (int i = 0; i < PlayerId.PlayersCount; i++)
            {
                perksCount.Add(players[i].Perks.CountReactive);
                shrineCount.Add(players[i].Shrines.CountReactive);
            }
            _countPerkAndShrine = new(perksCount, shrineCount);
            _unsubscribers += _countPerkAndShrine.Subscribe((perks, shrines) => _cursePerTurn = CURSE_PER_TURN + Mathf.Max(perks - CURSE_PER_CHRINE * shrines, 0));

        }

        public void Profit(int hexId)
        {
            if (hexId == ID)
                CurseAdd(CURSE_PROFIT);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();

            _countPerkAndShrine.Dispose();
            for (int i = 0; i <= _demons.Count; i++)
                _demons[i].Dispose();
        }

        private void CurseAdd(int value)
        {
            _curse += value;

            if (_curse < MAX_CURSE)
                return;

            _curse -= MAX_CURSE;
            _level++;
        }
    }
}
