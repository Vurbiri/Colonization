using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class PerkToggle : VToggleGraphic<PerkToggle>
    {
        [SerializeField, ReadOnly] private int _typePerkId;
        [SerializeField, ReadOnly] private int _perkId;
        [SerializeField, ReadOnly] private APerkHint _hint;

        private ReactiveCombination<int, int> _combination;
        private int _cost, _points;

        public void Init(PerkTree perkTree, Blood blood, Color colorLearn)
        {
            Perk perk = perkTree[_typePerkId, _perkId];
            _hint.Init(perk);

            if (perkTree.IsPerkLearned(_typePerkId, _perkId))
            {
                Learn(colorLearn);
            }
            else
            {
                _cost = perk.Cost;
                _points = perk.Points;

                _combination = new(perkTree.GetProgress(_typePerkId), blood, OnInteractable);
            }
        }

        public void BuyPerk(Color colorLearn)
        {
            var person = GameContainer.Person;
            if (person.CanLearnPerk(_typePerkId, _perkId, _cost))
            {
                Learn(colorLearn);
                person.BuyPerk(_typePerkId, _perkId, _cost);
            }
        }

        private void Learn(Color colorLearn)
        {
            transition = Transition.None;

            LeaveGroup();

            for (int i = _targetGraphics.Count - 1; i >= 0; i--)
                _targetGraphics[i].SetColor(Color.white);

            _checkmarkOn.canvasRenderer.SetColor(Color.white);
            _checkmarkOn.color = colorLearn;

            _hint.Learn();

            Destroy(_lockIcon.gameObject);
            Destroy(this);
        }

        private void OnInteractable(int progress, int blood)
        {
            InteractableAndUnlock(blood >= _cost, progress >= _points);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _combination?.Dispose();
        }

#if UNITY_EDITOR
        [HideInInspector] public RectTransform rectTransform;

        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetComponent(ref _hint);
            this.SetComponent(ref rectTransform);
        }

        public void Init_Editor(Perk perk, PerksWindow group)
        {
            UnityEditor.SerializedObject so = new(this);
            so.FindProperty(nameof(_typePerkId)).intValue = perk.Type;
            so.FindProperty(nameof(_perkId)).intValue = perk.Id;
            so.FindProperty(nameof(_group)).objectReferenceValue = group;
            so.ApplyModifiedProperties();

            var icon = this.GetComponentInChildren<Image>("Icon");
            so = new(icon);
            so.FindProperty("m_Sprite").objectReferenceValue = perk.sprite;
            so.ApplyModifiedProperties();

            bool enableModifier = perk.perkModifier == PerkModifierId.Enable;

            if (enableModifier)
            {
                icon.rectTransform.anchoredPosition = Vector2.zero;
                if(_hint is PerkHintValue)
                {
                    DestroyImmediate(_hint);
                    _hint = gameObject.AddComponent<PerkHint>();
                }
            }
            _hint.Init_Editor(perk);

            var text = GetComponentInChildren<TextMeshProUGUI>();
            if (text == null)
                return;

            if (enableModifier)
            {
                _targetGraphics.Remove(text);
                DestroyImmediate(text.gameObject);
                return;
            }

            so = new(text);
            var textProperty = so.FindProperty("m_text");
            if (perk.perkModifier == PerkModifierId.Percent)
                textProperty.stringValue = $"+{perk.Value}%";
            else
                textProperty.stringValue = perk.Value.ToString("+#;-#;0");
            so.ApplyModifiedProperties();
        }

#endif
    }
}
