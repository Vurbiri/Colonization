using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter))]
    public abstract class ASurfaceGenerator : MonoBehaviourDisposable
    {
        public abstract void Generate(float size);
    }
}
