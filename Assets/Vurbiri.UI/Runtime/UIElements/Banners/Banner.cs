using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.UI
{
	public class Banner
	{
        [SerializeField] private Image _backImage;
        [SerializeField] private TextMeshProUGUI _hintTMP;
        [Space]
        [SerializeField] private Vector2 _maxSize;
        [SerializeField] private Vector2 _padding;
        [Space]
        [SerializeField] private CanvasGroupSwitcher _waitSwitch;
        [SerializeField] private float _speedMove = 4f;

        protected RectTransform _backTransform;
        protected RectTransform _hintTransform;
        private Coroutine _coroutineShow, _coroutineHide;

        public virtual void Init()
        {
            _waitSwitch.Disable();

            _backTransform = _backImage.rectTransform;
            _hintTransform = _hintTMP.rectTransform;

            _hintTMP.enableWordWrapping = true;
            _hintTMP.overflowMode = TextOverflowModes.Overflow;
        }
    }
}
