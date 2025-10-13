using System;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
	public class PerksScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private ReadOnlyArray<Perk>[] _perks;

        public Perk this[int type, int id] => _perks[type][id];
        public ReadOnlyArray<Perk> this[int type] => _perks[type];

        public static implicit operator ReadOnlyArray<ReadOnlyArray<Perk>>(PerksScriptable self) => new(self._perks);

#if UNITY_EDITOR
        private void OnValidate()
        {
            _perks ??= new ReadOnlyArray<Perk>[2];
            if (_perks.Length != 2)
                Array.Resize(ref _perks, 2);
        }
#endif
    }
}
