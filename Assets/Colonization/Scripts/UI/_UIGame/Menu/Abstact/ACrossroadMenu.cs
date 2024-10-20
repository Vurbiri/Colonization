using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public abstract class ACrossroadMenu : MonoBehaviour
    {
        protected GameObject _thisGO;
        protected Players _players;
        protected Crossroad _currentCrossroad;

        protected virtual void Awake() => _thisGO = gameObject;

        public abstract void Open(Crossroad crossroad);

        public void Open() => _thisGO.SetActive(true);
        public void Close() => _thisGO.SetActive(false);
    }
}
