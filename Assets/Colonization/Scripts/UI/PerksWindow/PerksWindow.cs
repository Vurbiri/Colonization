using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public partial class PerksWindow : VToggleGroup<PerkToggle>
    {
        public void Init(Human player, CanvasHint hint)
        {
            PerkTree perkTree = player.Perks;
            var blood = player.Resources.Blood;

            foreach (var perk in _toggles)
            {
                perk.Init(perkTree, blood, hint);
            }
        }


#if UNITY_EDITOR

        

#endif
    }
}
