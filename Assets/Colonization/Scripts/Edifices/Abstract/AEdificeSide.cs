using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeSide : MonoBehaviour, IValueTypeEnum<LinkType>
    {
        [SerializeField] private LinkType _type;

        public LinkType Type => _type;

        protected virtual void Awake()
        {
            transform.localRotation = CONST.LINK_ROTATIONS[_type];
        }
    }
}
