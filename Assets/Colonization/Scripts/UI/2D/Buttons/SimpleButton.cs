using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
    public class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _target;
        [SerializeField] private Color _colorInside = Color.black;
        [SerializeField, Range(0.01f, 0.5f)] private float _fadeDuration = 0.1f;

        private readonly Color _colorNormal = Color.white;
        private readonly VAction _onClick = new();

        public Color Color { [Impl(256)] set => _target.color = value; }
        public bool Interactable { [Impl(256)] get; [Impl(256)] set; } = true;

        public Subscription AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Interactable && eventData.button == PointerEventData.InputButton.Left)
                _onClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _target.CrossFadeColor(_colorInside, _fadeDuration, true, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _target.CrossFadeColor(_colorNormal, _fadeDuration, true, true);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetComponent(ref _target);
        }
#endif
    }
}
