using UnityEngine;

[CreateAssetMenu(fileName = "Surface_", menuName = "Colonization/Surface", order = 51)]
public class SurfaceScriptable : ScriptableObject
{
    [SerializeField] private SurfaceType _type;
    [SerializeField] private Material _material;

    public SurfaceType Type => _type;
    public Material Material => _material;
}
