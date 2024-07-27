using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeSide : MonoBehaviour, IValueTypeEnum<LinkType>
    {
        [SerializeField] private LinkType _type;
        [Space]
        [SerializeField] protected int _idMaterial;

        public LinkType Type => _type;

        public virtual void SetActive(bool active) => gameObject.SetActive(active);

        public virtual void SetMaterial(Material material)
        {
            GetComponent<MeshRenderer>().SetSharedMaterial(material, _idMaterial);
        }
    }
}
