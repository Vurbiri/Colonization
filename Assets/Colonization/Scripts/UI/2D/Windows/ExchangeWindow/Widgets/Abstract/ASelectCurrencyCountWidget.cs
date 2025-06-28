using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASelectCurrencyCountWidget : MonoBehaviour
	{
        [SerializeField] protected Id<CurrencyId> _id;
        [Space]
        [SerializeField] protected int _min;
		[SerializeField] protected int _max;
		[Space]
		[SerializeField] protected TextMeshProUGUI _textValue;
		[SerializeField] protected SimpleHoldButton _leftButton;
		[SerializeField] protected SimpleHoldButton _rightButton;

        protected readonly Subscription<int, int> _changeCount = new();
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

        public void ResetCount()
        {
            if (_count != _min)
            {
                _count = _min;
                ValueToString();
                _leftButton.Interactable = false;
                _rightButton.Interactable = _min < _max;
            }
        }

        protected virtual void Awake()
		{
            SetValue(_min);

            _leftButton.AddListener(OnLeftClick);
			_rightButton.AddListener(OnRightClick);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetValue(int value)
		{
			_count = value;
            ValueToString();

            _leftButton.Interactable  = _count > _min;
            _rightButton.Interactable = _count < _max;

            _changeCount.Invoke(_id.Value, value);
        }

        protected virtual void ValueToString() => _textValue.text = _count.ToString();

        private void OnRightClick() => SetValue(_count + 1);
        private void OnLeftClick() => SetValue(_count - 1);

#if UNITY_EDITOR

        [SerializeField, HideInInspector] private UnityEngine.UI.Image _icon;
        private Rect _bounds;
        public Rect Bounds { get { SetBounds(); return _bounds; } }

        public virtual void Init_Editor(int id, Vector3 position)
        {
            transform.localPosition = position;

            UnityEditor.SerializedObject so = new(this);
            so.FindProperty("_id").FindPropertyRelative("_id").intValue = id;
            so.ApplyModifiedProperties();

            string name = $"{id}_{CurrencyId.Names[id]}";
            so = new(_icon);
            so.FindProperty("m_Sprite").objectReferenceValue = EUtility.FindMultipleSprite("SPA_C".Concat(name));
            so.ApplyModifiedProperties();

            gameObject.name = name;
        }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _textValue, "Value_TMP");
            this.SetChildren(ref _leftButton, "LeftButton");
            this.SetChildren(ref _rightButton, "RightButton");

            this.SetChildren(ref _icon, "Icon");

            _leftButton.CopyFrom(_rightButton);

            SetBounds();
        }

        private void SetBounds()
        {
            var mainSize = ((RectTransform)transform).sizeDelta;
            var buttonRect = _rightButton.RectTransformE;
            var iconRect = _icon.rectTransform;

            _bounds.size = mainSize;
            _bounds.width += (buttonRect.sizeDelta.x + Mathf.Abs(buttonRect.anchoredPosition.x)) * 2f;
            _bounds.height += iconRect.sizeDelta.y + Mathf.Abs(iconRect.anchoredPosition.y);

            if(_icon.transform.position.y > transform.position.y)
                _bounds.position = new(0f, (_bounds.height - mainSize.y) * 0.5f);
            else
                _bounds.position = new(0f, (mainSize.y - _bounds.height) * 0.5f);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(_bounds.position, _bounds.size);
        }
#endif
    }
}
