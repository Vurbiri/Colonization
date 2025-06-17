using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
	sealed public class PerkToggle : VToggle<PerkToggle>
    {
        [SerializeField, ReadOnly] private int _typePerkId;
        [SerializeField, ReadOnly] private int _perkId;
        [SerializeField, ReadOnly] private APerkHint _hint;
        [Space]
        [SerializeField] private Color _colorLearn;

        private ReactiveCombination<int, int, int> _combination;
        private int _cost, _points;

        public void Init(PerkTree perkTree, IReactive<int, int> blood, CanvasHint hint)
        {
            Perk perk = perkTree[_typePerkId, _perkId];
            _cost = perk.Cost;
            _points = perk.Points;

            _combination = new(perkTree.GetProgress(_typePerkId), blood, OnInteractable);

            _hint.Init(perk, hint);
        }

        private void OnInteractable(int progress, int blood, int deltaBlood)
        {
            interactable = progress >= _points & blood >= _cost;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

#if UNITY_EDITOR
            if (Application.isPlaying)
#endif
            _combination.Dispose();
        }

#if UNITY_EDITOR
        [Header("┌───────────── Editor ─────────────────────")]
        public RectTransform rectTransform;

        protected override void OnValidate()
        {
            base.OnValidate();

            EUtility.SetComponent(ref _hint, this);
            EUtility.SetComponent(ref rectTransform, this);
        }

        public void Init_Editor(Perk perk, PerksWindow group)
        {
            UnityEditor.SerializedObject so = new(this);
            so.FindProperty("_typePerkId").intValue = perk.Type;
            so.FindProperty("_perkId").intValue = perk.Id;
            so.FindProperty("_group").objectReferenceValue = group;
            so.ApplyModifiedProperties();

            var icon = EUtility.GetComponentInChildren<Image>(this, "Icon");
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
