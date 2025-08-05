using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class ASelectCurrencyCountWidget : MonoBehaviour
    {
        [SerializeField] protected Id<CurrencyId> _id;
        [Space]
        [SerializeField] protected int _max;
        [SerializeField] protected int _step;
        [Space]
        [SerializeField] protected TextMeshProUGUI _textValue;
        [Space]
        [SerializeField] private SimpleHoldButton _leftButton;
        [SerializeField] private SimpleHoldButton _rightButton;
        [Space]
        [SerializeField] private Image _panel;
        [SerializeField] private Image _icon;

        private Color _colorEnabled = Color.white, _colorDisabled;
        private float _fadeDuration;
        protected bool _interactable;
        protected int _count;

        public bool Interactable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _interactable & _max >= _step;
            set
            {
                if (_interactable != value)
                {
                    _interactable = value;
                    CrossFadeColor();
                    SetValue(0);
                }
            }
        }

        public void ResetCount()
        {
            _interactable = true;
            CrossFadeColor();
            SetValue(0);
        }

        protected virtual void Awake()
        {
            _fadeDuration = _rightButton.FadeDuration;
            _colorDisabled = _rightButton.DisabledColor;

            Interactable = true;

            _leftButton.AddListener(OnLeftClick);
            _rightButton.AddListener(OnRightClick);
        }

        public void SetMax(int value)
        {
            _max = value;
            CrossFadeColor();

            if (_count > _max)
            {
                value = _max - (_max % _step);
                SetValue(value);
            }
        }

        public void SetStep(int value)
        {
            _step = value;
            CrossFadeColor();

            value = _count - (_count % _step);
            if (_count != value)
                SetValue(value);
        }

        protected virtual void SetValue(int value)
        {
            _count = value;
            ValueToString();

            _leftButton.Interactable = (_count - _step) >= 0 & _interactable;
            _rightButton.Interactable = (_count + _step) <= _max & _interactable;
        }

        protected virtual void ValueToString() => _textValue.text = _count.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CrossFadeColor()
        {
            Color color = Interactable ? _colorEnabled : _colorDisabled;

            _textValue.CrossFadeColor(color, _fadeDuration, true, true);
            _panel.CrossFadeColor(color, _fadeDuration, true, true);
            _icon.CrossFadeColor(color, _fadeDuration, true, true);
        }

        private void OnRightClick() => SetValue(_count + _step);
        private void OnLeftClick() => SetValue(_count - _step);

#if UNITY_EDITOR
        private Rect _bounds;
        public Rect Bounds { get { SetBounds(); return _bounds; } }

        [SerializeField, StartEditor] private bool _centerY;

        public virtual void Init_Ed(int id, Vector3 position)
        {
            transform.localPosition = position;

            UnityEditor.SerializedObject so = new(this);
            so.FindProperty("_id").FindPropertyRelative("_id").intValue = id;
            so.ApplyModifiedProperties();

            string name = $"{id}_{CurrencyId.Names_Ed[id]}";
            so = new(_icon);
            so.FindProperty("m_Sprite").objectReferenceValue = EUtility.FindMultipleSprite("SPA_C".Concat(name));
            so.ApplyModifiedProperties();

            gameObject.name = name;
        }

        public void SetStep_Ed(int value)
        {
            if (value > 0)
            {
                UnityEditor.SerializedObject so = new(this);
                so.FindProperty("_step").intValue = value;
                so.ApplyModifiedProperties();
            }
        }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _textValue, "Value_TMP");
            this.SetChildren(ref _leftButton, "LeftButton");
            this.SetChildren(ref _rightButton, "RightButton");

            this.SetComponent(ref _panel);
            this.SetChildren(ref _icon, "Icon");

            _leftButton.CopyFrom_Editor(_rightButton);

            if (_step <= 0) _step = 1;
            if (_max < 0) _max = 0;

            SetBounds();
        }

        private void SetBounds()
        {
            var rectTransform = (RectTransform)transform;
            var mainSize = rectTransform.sizeDelta;
            var buttonRect = _rightButton.RectTransformE;
            var iconRect = _icon.rectTransform;

            _bounds.size = mainSize;
            _bounds.width += (buttonRect.sizeDelta.x + Mathf.Abs(buttonRect.anchoredPosition.x)) * 2f;
            _bounds.height += iconRect.sizeDelta.y + Mathf.Abs(iconRect.anchoredPosition.y);

            float y;
            if (_icon.transform.position.y > transform.position.y)
                y = (_bounds.height - mainSize.y) * 0.5f;
            else
                y = (mainSize.y - _bounds.height) * 0.5f;
            _bounds.position = new(0f, y);

            if (_centerY)
            {
                var position = rectTransform.anchoredPosition;
                position.y = -y;
                rectTransform.anchoredPosition = position;
            }
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
