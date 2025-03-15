//Assets\Colonization\Scripts\Players\Demon\Demon.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    using static GATE;

    public class Satan : IDisposable
    {
        private int _curse;
        private int _level;
        private int _cursePerTurn;

        private Unsubscribers _unsubscribers = new();

        private readonly DemonBuffs _leveling;
        private readonly Buffs _artefact;

        private readonly Hexagon _gateHex;
        private readonly DemonsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _demons = new();

        public Satan(Hexagon gateHex, Human[] players, SatanSaveData data, Players.Settings settings)
        {
            _gateHex = gateHex;

            _spawner = new(new(_leveling, _artefact), settings.demonPrefab, settings.actorsContainer, gateHex);
        }

        public void Profit(int hexId)
        {
            if (hexId == ID)
                CurseAdd(CURSE_PROFIT);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
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
