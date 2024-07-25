using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EdificesPrefabs", menuName = "Colonization/EdificesPrefabs", order = 51)]
public class EdificesScriptable : ScriptableObject, IEnumerable<AEdifice>
{
    [SerializeField] private EnumHashSet<EdificeType, AEdifice> _prefabs;
    [Space]
    [SerializeField] private EdificeType[] _buildTypes;

    public AEdifice this[EdificeType type] => _prefabs[type];
    public int Count => _prefabs.Count;
    public int Capacity => _prefabs.Capacity;
    public IEnumerable<EdificeType> BuildTypes => _buildTypes;


    public IEnumerator GetEnumerator() => _prefabs.GetEnumerator();
    IEnumerator<AEdifice> IEnumerable<AEdifice>.GetEnumerator() => _prefabs.GetEnumerator();
}
