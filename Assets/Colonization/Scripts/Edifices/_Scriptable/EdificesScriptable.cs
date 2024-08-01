using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "EdificesPrefabs", menuName = "Vurbiri/Colonization/EdificesPrefabs", order = 51)]
    public class EdificesScriptable : ScriptableObject, IEnumerable<AEdifice>
    {
        [SerializeField] private EnumHashSet<EdificeType, AEdifice> _prefabs;

        public AEdifice this[EdificeType type] => _prefabs[type];

        public IEnumerator GetEnumerator() => _prefabs.GetEnumerator();
        IEnumerator<AEdifice> IEnumerable<AEdifice>.GetEnumerator() => _prefabs.GetEnumerator();
    }
}
