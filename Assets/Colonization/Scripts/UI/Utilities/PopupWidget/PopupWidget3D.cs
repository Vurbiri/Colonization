//Assets\Colonization\Scripts\UI\Utilities\PopupWidget\PopupWidget3D.cs
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class PopupWidget3D : APopupWidget<TextMeshPro>
    {
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        private const string TEXT = "{0:+#;-#;0} <sprite={1}>";

        public void Init(int orderLevel)
        {
            _thisTMP.sortingOrder += orderLevel;
            base.Init(_directionPopup);
        }

        public void Run(int delta, int id)
        {
            if (delta == 0)
                return;

            Color start = _colorMinusStart, end = _colorMinusEnd;
            if (delta > 0)
            {
                start = _colorPlusStart;
                end = _colorPlusEnd;
            }

            _self.SetActive(true);
            _queue.Add(Run_Coroutine(string.Format(TEXT, delta, id), start, end));
        }
    }
}
