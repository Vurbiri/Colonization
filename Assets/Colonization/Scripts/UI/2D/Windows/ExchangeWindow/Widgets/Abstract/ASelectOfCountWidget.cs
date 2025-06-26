using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASelectOfCountWidget : MonoBehaviour
	{
		[SerializeField] protected int _min = 0;
		[SerializeField] protected int _max = 10;
		[Space]
		[SerializeField] private TextMeshProUGUI _textValue;
		[SerializeField] private SimpleHoldButton _leftButton;
		[SerializeField] private SimpleHoldButton _rightButton;
		[SerializeField] protected Image _icon;

        protected int _count;

        public int Count 
		{ 
			get => _count; 
			set
			{
				value = Mathf.Clamp(value, _min, _max);
				if(_count != value)
					SetValue(value);
			}
		}

        private void Awake()
		{
            SetValue(_min);

            _leftButton.AddListener(OnLeftClick);
			_rightButton.AddListener(OnRightClick);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void SetValue(int value)
		{
			_count = value;
            _textValue.text = _count.ToString();

            _leftButton.Interactable  = _count > _min;
            _rightButton.Interactable = _count < _max;
        }

        private void OnRightClick() => SetValue(_count + 1);
        private void OnLeftClick() => SetValue(_count - 1);

#if UNITY_EDITOR

        private Rect _size;
        public Rect Size { get { SetSize(); return _size; } }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _textValue);
            this.SetChildren(ref _leftButton, "LeftButton");
            this.SetChildren(ref _rightButton, "RightButton");
            this.SetChildren(ref _icon, "Icon");

            _leftButton.CopyFrom(_rightButton);

            SetSize();
        }

        protected virtual void SetSize()
        {
            var mainSize = ((RectTransform)transform).sizeDelta;
            var buttonRect = _rightButton.RectTransformE;
            var iconRect = _icon.rectTransform;

            _size.size = mainSize;
            _size.width += (buttonRect.sizeDelta.x + Mathf.Abs(buttonRect.anchoredPosition.x)) * 2f;
            _size.height += iconRect.sizeDelta.y + Mathf.Abs(iconRect.anchoredPosition.y);

            _size.position = new(0f, (_size.height - mainSize.y) * 0.5f);
        }

        public void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(_size.center - _size.size * 0.5f, _size.size);
        }
#endif
    }
}
