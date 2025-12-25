using UnityEngine;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class PlayerUINames
	{
        private readonly string[] _names = new string[PlayerId.Count];

        public string this[Id<PlayerId> id] { [Impl(256)] get => _names[id.Value]; }
        public string this[int index] { [Impl(256)] get => _names[index];}

        public PlayerUINames(PlayerNames names, PlayerColors colors)
        {
            names.Subscribe(Set, false);
            colors.Subscribe(Set, false);

            Set(names, colors);
        }

        private void Set(ReadOnlyArray<string> names) => Set(names, ProjectContainer.UI.PlayerColors);
        private void Set(ReadOnlyArray<Color32> colors) => Set(ProjectContainer.UI.PlayerNames, colors);

        [Impl(256)] private void Set(ReadOnlyArray<string> names, ReadOnlyArray<Color32> colors)
        {
            for (int i = 0; i < PlayerId.Count; ++i)
                _names[i] = $"<{colors[i].ToHex()}>{names[i]}</color>";
        }
    }
}
