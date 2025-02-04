//Assets\Colonization\Scripts\UI\Utilities\PopupWidget\PopupWidget3D.cs
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class PopupWidget3D : APopupWidget<TextMeshPro>
    {
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        private const string TAG_SPRITE = " <sprite={1}>";

        public void Init(int orderLevel)
        {
            _thisTMP.sortingOrder += orderLevel;
            
            base.Init(_directionPopup);

            _stringPlus = string.Concat(_stringPlus, TAG_SPRITE);
            _stringMinus = string.Concat(_stringMinus, TAG_SPRITE);
        }

        public void Run(int delta, int id)
        {
            if (delta == 0)
                return;

            _self.SetActive(true);
            _queue.Add(Run_Coroutine(string.Format(delta > 0 ? _stringPlus : _stringMinus, delta, id)));
        }
    }
}
