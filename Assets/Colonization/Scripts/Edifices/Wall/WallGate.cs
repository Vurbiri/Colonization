using UnityEngine;

namespace Vurbiri.Colonization
{
    public class WallGate : AEdificeSide
    {
        public override void AddRoad(Material material)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(material, _idMaterial);
            gameObject.SetActive(true);
        }
    }
}
