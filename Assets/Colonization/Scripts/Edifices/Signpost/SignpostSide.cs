using UnityEngine;

namespace Vurbiri.Colonization
{
    public class SignpostSide : AEdificeSide
    {
        [Space]
        [SerializeField, Range(0, 5)] protected int _idMaterial;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void AddRoad(Material material)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(material, _idMaterial);
        }
    }
}
