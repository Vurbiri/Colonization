using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    sealed public class BloodPanel : ASinglyPanel<CurrentMaxPopup>
    {
        public void Init(Vector3 directionPopup, Blood blood)
        {
            _widget.Init(directionPopup, blood);

            Destroy(this);
        }
    }
}
