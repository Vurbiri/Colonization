using UnityEngine;

namespace Vurbiri
{
    public abstract class ASceneEntryPoint : MonoBehaviour
    {
        public abstract void Run(IReadOnlyDIContainer projectContainer);
    }
}
