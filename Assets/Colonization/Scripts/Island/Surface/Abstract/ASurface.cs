using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ASurface : MonoBehaviour
    {
        public virtual void Init(bool oneFrame)
        {
            Destroy(this);
        }
    }
}
