//Assets\Colonization\Scripts\Players\Demon\Demon.cs
using System;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public class Demon : IDisposable
    {
        public const int GATE_DEFENSE = 3;


        protected readonly Hexagon _gateHex;
        protected readonly DemonsSpawner _spawner;
        protected readonly ListReactiveItems<Actor> _demons = new();

        public void Dispose()
        {
            for (int i = _demons.Count - 1; i >= 0; i--)
                _demons[i].Dispose();
        }
    }
}
