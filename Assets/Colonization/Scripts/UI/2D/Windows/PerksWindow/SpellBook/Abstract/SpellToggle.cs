using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public class SpellToggle : VToggleBase<SpellToggle>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, ReadOnly] private ASpellPanel _panel;
        [SerializeField, ReadOnly] private int _points;

        private readonly int _typeId, _id;
        private CanvasHint _hint;
        protected string _hintText;
        private bool _isShowingHint = false;
        private Vector3 _offsetHint;

        protected Transform _thisTransform;
       
        public int Type => _typeId;
        public int Id => _id;

        protected SpellToggle(int type, int id) : base() 
        {
            _typeId = type; _id = id;
            _points = id * (id + 1);
        }

        public void Init(PerkTree perkTree, SpellBook spellBook, Currencies resources, Action closeWindow)
        {
            _hint = GameContainer.UI.CanvasHint;
            _thisTransform = transform;
            _offsetHint = new(0f, ((RectTransform)_thisTransform).rect.size.y * 0.48f, 0f);

            perkTree.GetProgress(_typeId).Subscribe(OnInteractable);
            _panel.Init(spellBook, resources, closeWindow).OnHint += SetText;
        }

        private void SetText(string hintText) => _hintText = hintText;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (!_isShowingHint)
                _isShowingHint = _hint.Show(_hintText, _thisTransform.position, _offsetHint);
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
        sealed protected override void UpdateVisual() => _panel.Switch(_isOn);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        sealed protected override void UpdateVisualInstant() => _panel.SwitchInstant(_isOn);

        private void OnInteractable(int progress)
        {
            Interactable = progress >= _points;
        }

#if UNITY_EDITOR

        public void SetPosition_Ed(Vector2 position)
        {
            transform.localPosition = position;
            if (_panel != null)
                _panel.SetPosition_Ed(position);
        }

        protected override void OnValidate()
        {
            if (_panel == null)
            {
                foreach(var panel in FindObjectsByType<BloodTradePanel>(FindObjectsSortMode.None))
                {
                    if(panel.Type == _typeId & panel.Id == _id)
                    {
                        _panel = panel;
                        break;
                    }
                }
            }

            base.OnValidate();
        }
#endif
    }
}
