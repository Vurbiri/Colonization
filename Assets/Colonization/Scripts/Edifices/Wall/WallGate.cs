using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter))]
    public class WallGate : AEdificeSide
    {
        [Space]
        [SerializeField] private Mesh _meshGateOpen;

        public void AddRoad()
        {
            GetComponent<MeshFilter>().sharedMesh = _meshGateOpen;
        }
    }
}
