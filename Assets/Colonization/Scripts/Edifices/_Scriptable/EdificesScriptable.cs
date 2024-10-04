using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "EdificesPrefabs", menuName = "Vurbiri/Colonization/EdificesPrefabs", order = 51)]
    public class EdificesScriptable : ScriptableObject, IEnumerable<AEdifice>
    {
        [SerializeField] private IdHashSet<IdEdifice, AEdifice> _prefabs;

        public AEdifice this[int id] => _prefabs[id];

        public IEnumerator GetEnumerator() => _prefabs.GetEnumerator();
        IEnumerator<AEdifice> IEnumerable<AEdifice>.GetEnumerator() => _prefabs.GetEnumerator();
    }
}
