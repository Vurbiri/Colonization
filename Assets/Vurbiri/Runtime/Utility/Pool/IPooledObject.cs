//Assets\Vurbiri\Runtime\Utility\Pool\IPooledObject.cs
using UnityEngine;

namespace Vurbiri
{
    public interface IPooledObject<out T> where T : IPooledObject<T>
    {
        //public event Action<T, bool> EventDeactivate;

        public void ToPool(bool worldPositionStays = false);
        public void SetParent(Transform parent, bool worldPositionStays = false);

        public void SetActive(bool value);
        public void Enable();
        public void Disable();
    }
}
