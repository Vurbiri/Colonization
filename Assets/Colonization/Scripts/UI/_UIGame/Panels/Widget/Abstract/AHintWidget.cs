//Assets\Colonization\Scripts\UI\_UIGame\Panels\Widget\Abstract\AHintWidget.cs
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(UnityEngine.UI.Graphic))]
    public abstract class AHintWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected TextMeshProUGUI _valueTMP;
        [Space]
        [SerializeField] protected FileIdAndKey _getText;

        private CanvasHint _hint;
        private bool _isShowingHint = false;
        private Transform _thisTransform;
        protected Vector3 _offsetHint;

        protected string _text;

        protected void Init(ProjectColors colors, CanvasHint hint)
        {
            _valueTMP.color = colors.PanelText;
            _hint = hint;
            _thisTransform = transform;

            float offset = ((RectTransform)_thisTransform).rect.height * 0.5f;
            _offsetHint = new(0f, offset, 0f);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_text, _thisTransform.position, _offsetHint);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        private void OnDisable()
        {
            Hide();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Hide()
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }

#if UNITY_EDITOR
        public Vector2 Size => ((RectTransform)transform).rect.size;
        public void Init_Editor(ProjectColors colors)
        {
            _valueTMP.color = colors.PanelText;
        }

        protected virtual void OnValidate()
        {
            if (_valueTMP == null)
                _valueTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "TextTMP");
        }
#endif
    }
}
