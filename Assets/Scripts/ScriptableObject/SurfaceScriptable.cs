using UnityEngine;

[CreateAssetMenu(fileName = "Surface_", menuName = "Colonization/Surface", order = 51)]
public class SurfaceScriptable : ScriptableObject, IValueTypeEnum<SurfaceType>
{
    [SerializeField] private SurfaceType _type;
    [SerializeField] private Color32 _color;
    [Space]
    [SerializeField] private CurrencyType[] _profit;

    public SurfaceType Type => _type;
    public Color32 Color => _color;
    public bool IsWater => _type == SurfaceType.Water;
    public bool IsGate => _type == SurfaceType.Gate;

    public CurrencyType GetCurrency() => _profit.Rand();
}
