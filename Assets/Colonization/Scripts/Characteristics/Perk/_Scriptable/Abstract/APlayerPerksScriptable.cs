//Assets\Colonization\Scripts\Characteristics\Perk\_Scriptable\Abstract\APlayerPerksScriptable.cs
namespace Vurbiri.Colonization.Characteristics
{
    using Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Vurbiri.Colonization.UI;

    public class APlayerPerksScriptable<T> : ScriptableObject where T : APerkId<T>
    {
        [SerializeField] private IdArray<T, PerkSettings> _perks;

        public IReadOnlyList<IPerkSettings> Perks => _perks;
        public IReadOnlyList<IPerkSettingsUI> PerksUI => _perks;
    }
}
