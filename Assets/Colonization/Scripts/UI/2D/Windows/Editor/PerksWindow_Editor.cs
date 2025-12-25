#if UNITY_EDITOR

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
	public partial class PerksWindow
    {
        #region SerializeField
        [StartEditor]
        [SerializeField] private Vector2 _border = new(20f, 90f);
        [SerializeField, Range(20f, 60f)] private float _treeSpace = 48f;
        [Space]
        [SerializeField] private Vector2 _perkSpace = new(10.5f, 13.5f);
        [Space]
        [SerializeField] IdArray<EconomicSpellId, Sprite> _economicSpellSprites;
        [SerializeField] IdArray<EconomicSpellId, Vector2> _economicSpellSpritesSizeOffset;
        [SerializeField] IdArray<MilitarySpellId, Sprite> _militarySpellSprites;
        [SerializeField] IdArray<MilitarySpellId, Vector2> _militarySpellSpritesSizeOffset;
        [Space]
        [SerializeField, HideInInspector] private RectTransform _economicPerksContainer;
        [SerializeField, HideInInspector] private RectTransform _militaryPerksContainer;
        [SerializeField, HideInInspector] private RectTransform _economicSpellsContainer;
        [SerializeField, HideInInspector] private RectTransform _militarySpellsContainer;
        [SerializeField, HideInInspector] private RectTransform _separatorsContainer;

        [SerializeField, HideInInspector] private PerksScriptable _perks;
        [SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;

        [SerializeField, HideInInspector] private PerkToggle _perkPrefab;
        [SerializeField, HideInInspector] private SpellToggle _spellPrefab;
        [SerializeField, HideInInspector] private TextMeshProUGUI _separatorPrefab;

        [SerializeField, HideInInspector] private PerkToggle[] _economicPerks;
        [SerializeField, HideInInspector] private PerkToggle[] _militaryPerks;

        [SerializeField, HideInInspector] private SpellToggle[] _economicSpells;
        [SerializeField, HideInInspector] private SpellToggle[] _militarySpells;

        [SerializeField, HideInInspector] private TextMeshProUGUI[] _separators;
#pragma warning disable 414
        [SerializeField, EndEditor] private bool _endEditor;
#pragma warning restore 414
        #endregion

        private readonly int _countLevels = PerkTree.MAX_LEVEL + 1;

        private Vector2 PerkSize => _perkPrefab.rectTransform.sizeDelta + _perkSpace;

        public void UpdateVisuals_Ed(float pixelsPerUnit, SceneColorsEd colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.panelBack;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _closeButton.Color = colors.panelBack;
        }

        public void Setup_Ed()
        {
            print("[PerksWindow] Setup");

            SetupPerks_Ed(_economicPerks, AbilityTypeId.Economic, EconomicPerksId.Count, _economicPerksContainer);
            SetupPerks_Ed(_militaryPerks, AbilityTypeId.Military, MilitaryPerksId.Count, _militaryPerksContainer);

            float sizeX = _economicPerksContainer.sizeDelta.x + _treeSpace;
            Vector2 mainSize = new(sizeX * 2f, _economicPerksContainer.sizeDelta.y);
            
            GetComponent<RectTransform>().sizeDelta = mainSize + _border;

            sizeX += _treeSpace;
            _economicPerksContainer.anchoredPosition = new(sizeX * -0.5f, 0f);
            _militaryPerksContainer.anchoredPosition = new(sizeX * 0.5f, 0f);

            _separatorsContainer.sizeDelta = mainSize;

            Vector2 position = new(0f, PerkSize.y), current;
            Vector2 offset = new(0f, -_separatorsContainer.pivot.y * mainSize.y);
            SerializedObject so;
            for (int i = 0; i < _countLevels; i++)
            {
                current = position * i + offset;
                _separators[i].rectTransform.anchoredPosition = current;
                so = new(_separators[i]);
                so.FindProperty("m_text").stringValue = (i * (i + 1)).ToString();
                so.ApplyModifiedProperties();

                _economicSpells[i].Setup_Ed(current, _economicSpellSprites[i], _economicSpellSpritesSizeOffset[i]);
                _militarySpells[i].Setup_Ed(current, _militarySpellSprites[i], _militarySpellSpritesSizeOffset[i]);
            }
        }

        public void Create_Ed()
        {
            Delete_Ed();

            print("[PerksWindow] Create");

            CreatePerks_Ed(_economicPerks, EconomicPerksId.Count, _economicPerksContainer);
            CreatePerks_Ed(_militaryPerks, MilitaryPerksId.Count, _militaryPerksContainer);

            CreateSeparators_Ed();

            CreateSpells_Ed();

            Setup_Ed();
        }
        public void Delete_Ed()
        {
            print("[PerksWindow] Delete");
            
            DeleteT_Ed(_economicPerks, EconomicPerksId.Count, _economicPerksContainer);
            DeleteT_Ed(_militaryPerks, MilitaryPerksId.Count, _militaryPerksContainer);

            DeleteT_Ed(_economicSpells, EconomicSpellId.Count, _economicSpellsContainer);
            DeleteT_Ed(_militarySpells, MilitarySpellId.Count, _militarySpellsContainer);

            DeleteSeparators_Ed();
        }

        private void SetupPerks_Ed(PerkToggle[] perks, int typePerkId, int count, RectTransform container)
        {
            Vector2 perkSize = PerkSize;
            container.sizeDelta = perkSize * PerkTree.MAX_LEVEL;

            Vector2 offset = Vector2.zero - container.pivot;
            offset = container.sizeDelta * offset - perkSize * offset;

            Perk perk; PerkToggle perkToggle;
            for (int i = 0; i < count; i++)
            {
                perk = _perks[typePerkId, i];
                perkToggle = perks[i];

                Vector2 positionPerk = new(perk.position, perk.Level);
                perkToggle.rectTransform.anchoredPosition = perkSize * positionPerk + offset;
                perkToggle.Init_Editor(perk, _perksGroup);
            }
        }
        private void CreatePerks_Ed(PerkToggle[] perks, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
                perks[i] = EUtility.InstantiatePrefab(_perkPrefab, parent);
        }

        private void CreateSpells_Ed()
        {
            var panels = new Dictionary<SpellId, SpellPanel>(14);
            foreach (var panel in GetComponentsInChildren<SpellPanel>())
                panels.Add(panel.SpellId_Ed, panel);

            SpellId spellId = new(EconomicSpellId.Type, 0);
            for (spellId.id = 0; spellId.id < EconomicSpellId.Count; spellId.id++)
                _economicSpells[spellId.id] = EUtility.InstantiatePrefab(_spellPrefab, _economicSpellsContainer).Init_Editor(panels[spellId], _spellBookGroup);

            spellId.type = MilitarySpellId.Type;
            for (spellId.id = 0; spellId.id < MilitarySpellId.Count; spellId.id++)
                _militarySpells[spellId.id] = EUtility.InstantiatePrefab(_spellPrefab, _militarySpellsContainer).Init_Editor(panels[spellId], _spellBookGroup);
        }
        
        private void DeleteT_Ed<T>(T[] objects, int count, Transform parent) where T : MonoBehaviour
        {
            for (int i = 0; i < count; i++)
                EUtility.DestroyGameObject(ref objects[i]);

            objects = parent.GetComponentsInChildren<T>(true);
            for (int i = objects.Length - 1; i >= 0; i--)
                DestroyImmediate(objects[i].gameObject);
        }

        private void CreateSeparators_Ed()
        {
            for (int i = 0; i < _countLevels; i++)
                _separators[i] = EUtility.InstantiatePrefab(_separatorPrefab, _separatorsContainer);
        }

        private void DeleteSeparators_Ed()
        {
            for (int i = 0; i < _countLevels; i++)
                EUtility.DestroyGameObject(ref _separators[i]);

            while (_separatorsContainer.childCount > 0)
                Object.DestroyImmediate(_separatorsContainer.GetChild(0).gameObject);
              
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetChildren(ref _perksGroup);
            this.SetChildren(ref _spellBookGroup);

            this.SetChildren(ref _closeButton);

            _progressBars ??= new();

            this.SetChildren(ref _economicPerksContainer, "EconomicPerks");
            this.SetChildren(ref _militaryPerksContainer, "MilitaryPerks");
            this.SetChildren(ref _economicSpellsContainer, "EconomicSpellToggles");
            this.SetChildren(ref _militarySpellsContainer, "MilitarySpellToggles");
            this.SetChildren(ref _separatorsContainer, "Separators");

            EUtility.SetScriptable(ref _perks);
            EUtility.SetScriptable(ref _colorSettings);

            EUtility.SetPrefab(ref _perkPrefab);
            EUtility.SetPrefab(ref _spellPrefab);
            EUtility.SetPrefab(ref _separatorPrefab, "PUI_PerkSeparator");

            EUtility.SetArray(ref _economicPerks, EconomicPerksId.Count);
            EUtility.SetArray(ref _militaryPerks, MilitaryPerksId.Count);

            EUtility.SetArray(ref _economicSpells, EconomicSpellId.Count);
            EUtility.SetArray(ref _militarySpells, MilitarySpellId.Count);

            EUtility.SetArray(ref _separators, _countLevels);
        }
    }
}
#endif