using UnityEngine;

public class CityGate : ACityGraphicSide
{
    [SerializeField] private GameObject _openGate;
    [SerializeField] private GameObject _closeGate;

    public override void Initialize()
    {
        base.Initialize();
        
        _openGate.SetActive(false);
        _closeGate.SetActive(true);
    }

    public override void RoadBuilt() 
    {
        _openGate.SetActive(true);
        _closeGate.SetActive(false);
    }
}
