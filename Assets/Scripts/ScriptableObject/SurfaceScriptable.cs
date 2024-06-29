using UnityEngine;

[CreateAssetMenu(fileName = "Surface_", menuName = "Colonization/Surface", order = 51)]
public class SurfaceScriptable : ScriptableObject
{
    [SerializeField] private Resource _type;
    [SerializeField] private Color32 _color;

    public Resource Type => _type;
    public Color32 Color => _color;
}
