using UnityEngine;

public class CityGate : CityGraphicSide
{
    [SerializeField] private GameObject _openGate;
    [SerializeField] private GameObject _closeGate;

    public override void Initialize()
    {
        base.Initialize();
        
        _openGate.SetActive(false);
        _closeGate.SetActive(true);
    }

    public override void SetMaterial(Material material) 
    {
        _openGate.SetActive(true);
        _closeGate.SetActive(false);

        base.SetMaterial(material);
    }
}
