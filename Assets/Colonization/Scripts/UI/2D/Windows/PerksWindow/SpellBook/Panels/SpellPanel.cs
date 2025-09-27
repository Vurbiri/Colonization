using System;
using UnityEngine;
using Vurbiri.UI;
using static Vurbiri.Colonization.SpellBook;

namespace Vurbiri.Colonization.UI
{
	public class SpellPanel : MonoBehaviour
	{
        [SerializeField] protected SpellId _spellId;
        [SerializeField] protected CanvasGroupSwitcher _switcher;
        [SerializeField] protected VButton _applyButton;

        protected SpellParam _spellParam = new(PlayerId.Person);
        protected ASpell _spell;

        public int Type => _spellId.type;
        public int Id => _spellId.id;

        public ASpell Init(SpellBook spellBook, Action closeWindow)
        {
            InitInternal(spellBook, closeWindow);

            return _spell;
        }

        protected virtual void InitInternal(SpellBook spellBook, Action closeWindow)
        {
            _spell = spellBook[_spellId];

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

        public int Points_Ed => _spellId.id * (_spellId.id + 1);
        public SpellId SpellId_Ed => _spellId;

        public string Setup_Ed(Vector2 position)
        {
            transform.localPosition = position;

            string skillName = $"{_spellId.id}_{(_spellId.type == EconomicSpellId.Type ? EconomicSpellId.Names_Ed[_spellId.id] : MilitarySpellId.Names_Ed[_spellId.id])}";
            gameObject.name = skillName.Concat("Panel");
            return skillName;
        }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _applyButton);
            _switcher?.OnValidate(this, 6);
        }
#endif
    }
}
