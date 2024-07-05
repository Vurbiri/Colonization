using UnityEngine;

[CreateAssetMenu(fileName = "Surface_", menuName = "Colonization/Surface", order = 51)]
public class SurfaceScriptable : ScriptableObject, IValueTypeEnum<CurrencyType>
{
    [SerializeField] private CurrencyType _type;
    [SerializeField] private Color32 _color;

    public CurrencyType Type => _type;
    public Color32 Color => _color;
}
