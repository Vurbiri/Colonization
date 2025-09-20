using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Characteristics
{
	//[CreateAssetMenu(fileName = "Perks", menuName = "Vurbiri/Colonization/PerksScriptable", order = 51)]
	public class PerksScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private ReadOnlyArray<Perk>[] _perks;

        public Perk this[int type, int id]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _perks[type][id];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
