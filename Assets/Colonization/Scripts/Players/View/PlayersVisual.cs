//Assets\Colonization\Scripts\Players\View\PlayersVisual.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public class PlayersVisual
    {
        private readonly PlayerVisual[] _playersVisual = new PlayerVisual[PlayerId.Count];

        public PlayerVisual this[int index] => _playersVisual[index];
        public PlayerVisual this[Id<PlayerId> id] => _playersVisual[id.Value];

        public PlayersVisual(Color[] colors, Material materialLit, Material materialUnlit, Material materialWarriors)
        {
            int i = 0;
            for (; i < PlayerId.HumansCount; i++)
                _playersVisual[i] = new(colors[i], new(materialLit), new(materialUnlit), new(materialWarriors));

            _playersVisual[i] = new(colors[i]);
        }
    }
}
