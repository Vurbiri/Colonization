using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
	public class PlayerUINames : IDisposable
	{
        private readonly string[] _names = new string[PlayerId.Count];
        private readonly ReactiveCombination<PlayerColors, PlayerNames> _combination;

        public string this[Id<PlayerId> id] => _names[id.Value];
        public string this[int index] => _names[index];

        public PlayerUINames(PlayerColors playerColors, PlayerNames names) => _combination = new(playerColors, names, Set);

        private void Set(PlayerColors colors, PlayerNames names)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _names[i] = $"<{colors[i].ToHex()}>{names[i]}</color>";
        }

        public void Dispose() => _combination.Dispose();
    }
}
