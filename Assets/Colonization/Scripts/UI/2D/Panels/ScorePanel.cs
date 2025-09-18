using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    sealed public class ScorePanel : ASinglyPanel<ScorePopup>
    {
        public void Init(Vector3 directionPopup)
        {
            _widget.Init(directionPopup);
            Destroy(this);
        }
    }
}
