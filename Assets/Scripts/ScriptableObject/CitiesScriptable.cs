using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CitiesPrefabs", menuName = "Colonization/CitiesPrefabs", order = 51)]
public class CitiesScriptable : ScriptableObject
{
    [SerializeField] private List<ACity> _prefabs;

    public ACity this[CityType type] => _prefabs[(int)type];

#if UNITY_EDITOR
    private void OnValidate()  => _prefabs.Sort();
#endif
}
