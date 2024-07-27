using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public abstract class ACrossroadMenu : MonoBehaviour
    {
        protected Players _players;
        protected Crossroad _currentCrossroad;

        public abstract void Open(Crossroad crossroad);

        public void Open() => gameObject.SetActive(true);
        public void Close() => gameObject.SetActive(false);
    }
}
