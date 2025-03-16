//Assets\Colonization\Scripts\Players\Demon\Demon.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public class Satan : IDisposable
    {
        private int _curse;
        private int _level;
        private int _cursePerTurn;

        private Unsubscribers _unsubscribers = new();

        private readonly SatanAbilities _states;

        private readonly DemonBuffs _leveling;
        private readonly Buffs _artefact;

        private readonly Hexagon _gateHex;
        private readonly DemonsSpawner _spawner;
        private readonly ListReactiveItems<Actor> _demons = new();

        private int MaxCurse => _states.maxCurse + _level * _states.maxCursePerLevel;

        public Satan(SatanSaveData data, Players.Settings settings)
        {
            _gateHex = SceneObjects.Get<Hexagons>()[Key.Zero];
            _states = settings.satanStates;

            _spawner = new(new(_leveling, _artefact), settings.demonPrefab, settings.actorsContainer, _gateHex);
        }

        public void Profit(int hexId)
        {
            if (hexId == GATE.ID)
                CurseAdd(_states.curseProfit);
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

            if (_curse < MaxCurse)
                return;

            _curse -= MaxCurse;
            _level++;
        }
    }
}
