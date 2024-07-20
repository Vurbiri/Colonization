using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class WaterGraphic : ACityGraphic
{
    [SerializeField] private int _idMaterial;

    private static readonly EnumArray<LinkType, Quaternion> angles = new(
        new Quaternion[] { Quaternion.Euler(0f, 120f, 0f), Quaternion.Euler(0f, 240f, 0f), Quaternion.Euler(0f, 0f, 0f) });
    private static readonly EnumArray<LinkType, Quaternion> anglesMirror = new(
        new Quaternion[] { Quaternion.Euler(0f, 300, 0f), Quaternion.Euler(0f, 60f, 0f), Quaternion.Euler(0f, 180f, 0f) });

    public override void Initialize() { }


    public override void Upgrade(PlayerType owner, EnumHashSet<LinkType, CrossroadLink> links)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material[] materials = renderer.sharedMaterials;
        materials[_idMaterial] = Players.Instance[owner].Material; 
        renderer.sharedMaterials = materials;

        if (links.Count == 3)
        {
            foreach (var link in links)
            {
                if (!link.IsWater)
                {
                    transform.localRotation = anglesMirror[link.Type];
                    break;
                }
            }
        }
        else
        {
            transform.localRotation = angles[links.FirstEmptyIndex()];
        }
    }

    public override void AddLink(LinkType type) {}

    public override void RoadBuilt(LinkType type, PlayerType owner) { }
}
