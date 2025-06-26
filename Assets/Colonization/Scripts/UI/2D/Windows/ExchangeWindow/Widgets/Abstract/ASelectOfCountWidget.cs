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
		[SerializeField] private Image _icon;

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

        protected virtual void OnValidate()
        {
			EUtility.SetChildren(ref _textValue, this);
			EUtility.SetChildren(ref _leftButton, this, "LeftButton");
			EUtility.SetChildren(ref _rightButton, this, "RightButton");
			EUtility.SetChildren(ref _icon, this);

            _leftButton.CopyFrom_Editor(_rightButton);
        }
#endif
    }
}
