using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public abstract class ACrossroadMenuBuild : ACrossroadMenu
    {
        [SerializeField] protected float _distanceOfButtons = 5f;

        protected Players _players;
        protected Player _playerCurrent;

    }
}
