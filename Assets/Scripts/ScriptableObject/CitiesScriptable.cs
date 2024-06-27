using UnityEngine;

[CreateAssetMenu(fileName = "CitiesPrefabs", menuName = "Colonization/CitiesPrefabs", order = 51)]
public class CitiesScriptable : ScriptableObject
{
    [SerializeField] private EnumHashSet<CityType, City> _prefabs;

    public City this[CityType type] => _prefabs[type];
}
