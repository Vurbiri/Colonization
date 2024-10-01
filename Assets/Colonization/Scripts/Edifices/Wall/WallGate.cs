using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter))]
    public class WallGate : AEdificeSide
    {
        [Space]
        [SerializeField] private MeshFilter _thisMeshFilter;
        [Space]
        [SerializeField] private Mesh _meshGateOpen;
        [SerializeField] private Mesh _meshGateClose;

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
