//Assets\Vurbiri\Runtime\Types\Disposable\MonoBehaviourDisposable.cs
namespace Vurbiri
{
    public class MonoBehaviourDisposable : UnityEngine.MonoBehaviour, System.IDisposable
    {
        public virtual void Dispose()
        {
            Destroy(this);
        }
    }
}
