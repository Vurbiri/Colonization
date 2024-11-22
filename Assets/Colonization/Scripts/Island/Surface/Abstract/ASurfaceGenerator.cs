//Assets\Colonization\Scripts\Island\Surface\Abstract\ASurfaceGenerator.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshFilter))]
    public abstract class ASurfaceGenerator : MonoBehaviourDisposable
    {
        public abstract IEnumerator Generate_Coroutine(float size);
    }
}
