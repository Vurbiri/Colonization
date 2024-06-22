using UnityEngine;

public class NoneBuilding : ABuilding
{
    [Space]
    [SerializeField] private BuildingsScriptable _prefabs;

    public override BuildingType Type => BuildingType.None;

    public override void Setup()
    {
        base.Setup();

        _prefabNextUpgrade = _isGate ? _prefabs[BuildingType.Shrine] : _prefabs[BuildingType.Camp];
    }
}
