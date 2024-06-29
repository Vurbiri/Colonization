using UnityEngine;

public class ShrineSide : CityGraphicSide
{
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _cross;

    public override void Initialize()
    {
        _wall.SetActive(true);
        _cross.SetActive(false);
    }

    public override void AddLink()
    {
        _wall.SetActive(false);
        _cross.SetActive(true);
    }
}
