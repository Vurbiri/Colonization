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
        [SerializeField] protected int _min;
        [SerializeField] protected int _max;
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
            get => _interactable;
            set
            {
                if (_interactable != value)
                {
                    _interactable = value;
                    CrossFadeColor();
                    SetValue(_min);
                }
            }
        }

        public void ResetCount()
        {
            Interactable = true;
            if (_count != _min)
                SetValue(_min);
        }

        protected virtual void Awake()
        {
            _fadeDuration = _rightButton.FadeDuration;
            _colorDisabled = _rightButton.ColorDisabled;

            Interactable = true;

            _leftButton.AddListener(OnLeftClick);
            _rightButton.AddListener(OnRightClick);
        }

        protected virtual void SetValue(int value)
        {
            _count = value;
            ValueToString();

            _leftButton.Interactable = _count > _min & _interactable;
            _rightButton.Interactable = _count < _max & _interactable;
        }

        protected virtual void ValueToString() => _textValue.text = _count.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void CrossFadeColor()
        {
            Color color = _interactable & _max > _min ? _colorEnabled : _colorDisabled;

            _textValue.CrossFadeColor(color, _fadeDuration, true, true);
            _panel.CrossFadeColor(color, _fadeDuration, true, true);
            _icon.CrossFadeColor(color, _fadeDuration, true, true);
        }

        private void OnRightClick() => SetValue(_count + 1);
        private void OnLeftClick() => SetValue(_count - 1);

#if UNITY_EDITOR
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

            this.SetComponent(ref _panel);
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

            if (_icon.transform.position.y > transform.position.y)
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
