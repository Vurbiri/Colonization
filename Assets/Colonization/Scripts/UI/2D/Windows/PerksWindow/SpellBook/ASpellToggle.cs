using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASpellToggle : VToggleAlpha<ASpellToggle>
	{
        [SerializeField] private int _typePerkId;
        [SerializeField] private int _spellId;
    }
}
