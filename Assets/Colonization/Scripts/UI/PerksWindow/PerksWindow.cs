using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class PerksWindow : VToggleGroup<PerkToggle>
    {
        [SerializeField] private HintButton _learnButton;
        [Space]
        [SerializeField] private Color _colorLearn;

        private Human _player;

        public void Init(Human player, CanvasHint hint)
        {
            _player = player;

            _learnButton.Init(hint);

            PerkTree perkTree = player.Perks;
            var blood = player.Resources.Blood;

            foreach (var perk in _toggles)
                perk.Init(perkTree, blood, hint);
        }

    }
}
