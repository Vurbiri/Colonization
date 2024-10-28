using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public abstract class AEdificeSide : MonoBehaviour, IValueId<LinkId>
    {
        [SerializeField] private Id<LinkId> _id;

        public Id<LinkId> Id => _id;

        protected virtual void Awake()
        {
            transform.localRotation = CONST.LINK_ROTATIONS[_id.Value];
        }
    }
}
