using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingPrefabs", menuName = "Colonization/BuildingPrefabs", order = 51)]
public class BuildingsScriptable : ScriptableObject
{
    [SerializeField] private List<ABuilding> _prefabs;

    public ABuilding this[BuildingType type]
    {
        get 
        { 
            foreach (var building in _prefabs) 
                if(building.Type == type)
                    return building;

            return null;
        }
    }
}
