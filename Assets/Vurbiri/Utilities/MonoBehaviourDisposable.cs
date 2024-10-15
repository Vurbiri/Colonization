using System;
using UnityEngine;

namespace Vurbiri
{
    public class MonoBehaviourDisposable : MonoBehaviour, IDisposable
    {
        public virtual void Dispose()
        {
            Destroy(this);
        }
    }
}
