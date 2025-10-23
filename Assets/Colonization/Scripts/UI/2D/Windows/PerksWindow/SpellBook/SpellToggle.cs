using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class SpellToggle : VToggleBase<SpellToggle>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, ReadOnly] private SpellPanel _panel;
        [SerializeField, ReadOnly] private int _points;

        private CanvasHint _hint;
        private string _hintText;
        private bool _isShowingHint = false;
        private Vector3 _offsetHint;

        public void Init(PerkTree perkTree, SpellBook spellBook, Action closeWindow)
        {
            _hint = GameContainer.UI.CanvasHint;
            if (_thisRectTransform == null)
                _thisRectTransform = (RectTransform)transform;

            _offsetHint = new(0f, _thisRectTransform.rect.size.y * 0.48f, 0f);

            perkTree.GetProgress(_panel.Type).Subscribe(OnInteractable);
            _panel.Init(spellBook, closeWindow).OnHint += SetText;
        }

        private void SetText(string hintText) => _hintText = hintText;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_hintText, _thisRectTransform, _offsetHint);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Hide();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Hide();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Hide()
        {
            if (_isShowingHint)
                _isShowingHint = !_hint.Hide();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisual()
        {
#if UNITY_EDITOR
            if (_panel != null)
#endif
                _panel.Switch(_isOn);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisualInstant()
        {
#if UNITY_EDITOR
            if (_panel != null)
#endif
                _panel.SwitchInstant(_isOn);
        }

        private void OnInteractable(int progress)
        {
            Unlock = progress >= _points;
        }

#if UNITY_EDITOR

        public void Setup_Ed(Vector2 position, Sprite sprite, Vector2 sizeOffset)
        {
            transform.localPosition = position;
            gameObject.name = _panel.Setup_Ed(position);

            var icon = this.GetComponentInChildren<Image>("Icon");
            UnityEditor.SerializedObject so = new(icon);
            so.FindProperty("m_Sprite").objectReferenceValue = sprite;
            so.ApplyModifiedProperties();

            icon.rectTransform.sizeDelta = new(sizeOffset.x, sizeOffset.x);
            icon.rectTransform.anchoredPosition = new(0f, sizeOffset.y);
        }

        public SpellToggle Init_Editor(SpellPanel panel, SpellBookGroup group)
        {
            UnityEditor.SerializedObject so = new(this);
            so.FindProperty(nameof(_points)).intValue = panel.Points_Ed;
            so.FindProperty(nameof(_panel)).objectReferenceValue = panel;
            so.FindProperty(nameof(_group)).objectReferenceValue = group;
            so.ApplyModifiedProperties();

            return this;
        }
#endif
    }
}
