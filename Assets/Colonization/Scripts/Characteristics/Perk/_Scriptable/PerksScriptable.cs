using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
	public class PerksScriptable : ScriptableObject
    {
        public const string NAME = "Perks";
        
        [SerializeField] private ReadOnlyArray<Perk>[] _perks;

        public Perk this[int type, int id] => _perks[type][id];
        public ReadOnlyArray<Perk> this[int type] => _perks[type];

        public static implicit operator ReadOnlyArray<ReadOnlyArray<Perk>>(PerksScriptable self) => new(self._perks);

#if UNITY_EDITOR

        private bool _renaming_ed;
        
        private void OnValidate()
        {
            _perks ??= new ReadOnlyArray<Perk>[2];
            if (_perks.Length != 2)
                Array.Resize(ref _perks, 2);

            if(name != NAME & !_renaming_ed && !Application.isPlaying)
                Rename_Ed();
        }

        private async void Rename_Ed()
        {
            _renaming_ed = true;

            await System.Threading.Tasks.Task.Delay(10);
            if (!Application.isPlaying)
            {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), NAME);
                AssetDatabase.SaveAssets();
            }
            _renaming_ed = false;
        }
#endif
    }
}
