using UnityEngine;

namespace Vurbiri.Colonization
{
    public class PlayersVisual
    {
        private readonly PlayerVisual[] _playersView = new PlayerVisual[PlayerId.CountPlayers];

        public PlayerVisual this[int index] => _playersView[index];
        public PlayerVisual this[Id<PlayerId> id] => _playersView[id.Value];

        public PlayersVisual(Color[] colors, Material materialLit, Material materialUnlit, Material materialWarriors)
        {
            for (int i = 0; i < PlayerId.CountPlayers; i++)
                _playersView[i] = new(colors[i], new(materialLit), new(materialUnlit), new(materialWarriors));
        }
    }
}
