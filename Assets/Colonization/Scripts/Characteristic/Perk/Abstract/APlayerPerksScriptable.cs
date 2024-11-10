namespace Vurbiri.Colonization
{
    using Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class APlayerPerksScriptable<T> : ScriptableObject where T : APerkId<T>
    {
        [SerializeField] private IdArray<T, PerkSettings> _perks;

        public IReadOnlyList<IPerkSettings> Perks => _perks;
        public IReadOnlyList<IPerkSettingsUI> PerksUI => _perks;
    }
}
