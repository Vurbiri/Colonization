//Assets\Colonization\Scripts\Characteristics\Perk\_Scriptable\Abstract\APlayerPerksScriptable.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class APlayerPerksScriptable<T> : ScriptableObjectDisposable where T : APerkId<T>
    {
        [SerializeField] private IdArray<T, Perk> _perks;

        public IReadOnlyList<Perk> Perks => _perks;
    }
}
