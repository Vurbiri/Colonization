//Assets\Colonization\Scripts\Edifices\Wall\WallGate.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter))]
    public class WallGate : MonoBehaviour, IValueId<LinkId>
    {
        [SerializeField] private Id<LinkId> _id;
        [Space]
        [SerializeField] private MeshFilter _thisMeshFilter;
        [Space]
        [SerializeField] private Mesh _meshGateOpen;
        [SerializeField] private Mesh _meshGateClose;

        public Id<LinkId> Id => _id;

        private void Awake()
        {
            transform.localRotation = CONST.LINK_ROTATIONS[_id.Value];
        }

        public void Open(bool isOpen)
        {
            _thisMeshFilter.sharedMesh = isOpen ? _meshGateOpen : _meshGateClose;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_thisMeshFilter == null)
                _thisMeshFilter = GetComponent<MeshFilter>();
        }
#endif
    }
}
