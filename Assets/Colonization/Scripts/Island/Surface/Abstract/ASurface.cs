//Assets\Colonization\Scripts\Island\Surface\Abstract\ASurface.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract class ASurface : MonoBehaviour
    {
        public virtual void Init()
        {
            Destroy(this);
        }
    }
}
