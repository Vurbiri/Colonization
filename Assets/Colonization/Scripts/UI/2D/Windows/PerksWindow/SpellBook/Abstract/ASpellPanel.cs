using System;
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	public abstract class ASpellPanel : MonoBehaviour
	{
        [SerializeField] private CanvasGroupSwitcher _switcher;
        [SerializeField] protected VButton _applyButton;

        protected readonly int _typeId, _id;
        protected readonly SpellParam _spellParam = new(PlayerId.Person);
        protected SpellBook.ASpell _spell;

        public int Type => _typeId;
        public int Id => _id;

        protected ASpellPanel(int type, int id) : base() 
        {
            _typeId = type; _id = id;
        }

        public virtual void Init(SpellBook spellBook, Currencies resources, Action closeWindow)
        {
            _spell = spellBook[_typeId, _id];

            _applyButton.AddListener(closeWindow);
            _applyButton.AddListener(Apply);

            _applyButton.interactable = false;
        }

        public void Switch(bool show)
        {
            if (_switcher.IsRunning)
                StopCoroutine(_switcher);

            if (show)
            {
                StartCoroutine(_switcher.Show());
                StateReset();
            }
            else
            {
                StartCoroutine(_switcher.Hide());
            }
        }

        public void SwitchInstant(bool show) => _switcher.Set(show);

        protected virtual void StateReset()
        {
            _spellParam.Reset();
            _applyButton.interactable = _spell.Prep(_spellParam);
        }

        private void Apply()
        {
            _spell.Cast(_spellParam);
        }

#if UNITY_EDITOR
        
        public void SetPosition_Ed(Vector2 position)
        {
            var rectTransform = (RectTransform)transform;
            position.x = rectTransform.sizeDelta.x * 0.5f - 1f;
            rectTransform.anchoredPosition = position;
        }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _applyButton);
            _switcher.OnValidate(this, 6);
        }
#endif
    }
}
