using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CitiesPrefabs", menuName = "Colonization/CitiesPrefabs", order = 51)]
public class CitiesScriptable : ScriptableObject, IEnumerable<City>
{
    [SerializeField] private EnumHashSet<CityType, City> _prefabs;

    public City this[CityType type] => _prefabs[type];
    public int Count => _prefabs.Count;
    public int Capacity => _prefabs.Capacity;

    public object Current => throw new System.NotImplementedException();

    public IEnumerator GetEnumerator() => _prefabs.GetEnumerator();
    IEnumerator<City> IEnumerable<City>.GetEnumerator() => _prefabs.GetEnumerator();
}
