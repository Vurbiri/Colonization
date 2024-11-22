//Assets\Vurbiri\Runtime\Types\Disposable\ScriptableObjectDisposable.cs
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
