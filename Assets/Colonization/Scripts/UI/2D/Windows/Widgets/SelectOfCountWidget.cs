using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
	public class SelectOfCountWidget : MonoBehaviour
	{
		[SerializeField] private int _min = 0;
		[SerializeField] private int _max = 10;
		[Space]
		[SerializeField] private TextMeshProUGUI _textValue;
		[SerializeField] private SimpleHoldButton _leftButton;
		[SerializeField] private SimpleHoldButton _rightButton;
		[SerializeField] private Image _icon;

		private int _count;

		private void Awake()
		{
            SetValue(_min);

            _leftButton.AddListener(Decrement);
			_rightButton.AddListener(Increment);
        }

		private void Increment() => SetValue(_count + 1);
        private void Decrement() => SetValue(_count - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetValue(int value)
		{
			_count = value;
            _textValue.text = value.ToString();

            _leftButton.Interactable  = value > _min;
            _rightButton.Interactable = value < _max;
        }

		
#if UNITY_EDITOR
        private void OnValidate()
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
