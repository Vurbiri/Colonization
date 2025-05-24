using UnityEngine;

namespace Vurbiri
{
    public class ScriptableObjectDisposable : ScriptableObject, System.IDisposable
    {
        public virtual void Dispose()
        {
            Resources.UnloadAsset(this);
        }
    }
}
