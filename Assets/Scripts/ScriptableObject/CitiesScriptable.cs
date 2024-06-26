using UnityEngine;

[CreateAssetMenu(fileName = "CitiesPrefabs", menuName = "Colonization/CitiesPrefabs", order = 51)]
public class CitiesScriptable : ScriptableObject
{
    [SerializeField] private EnumArray<CityType, ACity> _prefabs;

    public ACity this[CityType type] => _prefabs[type];
}
