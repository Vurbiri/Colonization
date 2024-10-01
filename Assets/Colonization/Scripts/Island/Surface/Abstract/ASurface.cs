using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ASurface : MonoBehaviour
    {
        public virtual void Initialize()
        {
            Destroy(this);
        }
    }
}
