#if UNITY_EDITOR

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

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

        public void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            Color color = colors.PanelBack.SetAlpha(1f);
            Image image = GetComponent<Image>();
            image.color = color;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _closeButton.Color = color;
        }

        public void Setup_Ed()
        {
            SetupPerks_Ed(_economicPerks, TypeOfPerksId.Economic, EconomicPerksId.Count, _economicPerksContainer);
            SetupPerks_Ed(_militaryPerks, TypeOfPerksId.Military, MilitaryPerksId.Count, _militaryPerksContainer);

            float sizeX = _economicPerksContainer.sizeDelta.x + _treeSpace;
            Vector2 mainSize = new(sizeX * 2f, _economicPerksContainer.sizeDelta.y);
            
            GetComponent<RectTransform>().sizeDelta = mainSize + _border;

            sizeX += _treeSpace;
            _economicPerksContainer.anchoredPosition = new(sizeX * -0.5f, 0f);
            _militaryPerksContainer.anchoredPosition = new(sizeX * 0.5f, 0f);

            _separatorsContainer.sizeDelta = mainSize;

            Vector2 position = new(0f, PerkSize.y), current;
            Vector2 offset = new(0f, -_separatorsContainer.pivot.y * mainSize.y);
            SerializedObject so;;
            for (int i = 0; i < _countLevels; i++)
            {
                current = position * i + offset;
                _separators[i].rectTransform.anchoredPosition = current;
                so = new(_separators[i]);
                so.FindProperty("m_text").stringValue = (i * (i + 1)).ToString();
                so.ApplyModifiedProperties();

                if(_economicSpells[i] != null)
                    _economicSpells[i].SetPosition_Ed(current);
                if (_militarySpells[i] != null)
                    _militarySpells[i].SetPosition_Ed(current);
            }
        }

        public void Create_Ed()
        {
            Delete_Ed();

            CreatePerks_Ed(_economicPerks, EconomicPerksId.Count, _economicPerksContainer);
            CreatePerks_Ed(_militaryPerks, MilitaryPerksId.Count, _militaryPerksContainer);

            CreateSeparators_Ed();

            CreateSpells_Ed();

            Setup_Ed();
        }
        public void Delete_Ed()
        {
            DeletePerks_Ed(_economicPerks, EconomicPerksId.Count, _economicPerksContainer);
            DeletePerks_Ed(_militaryPerks, MilitaryPerksId.Count, _militaryPerksContainer);

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
                perk = _perks[typePerkId][i];
                perkToggle = perks[i];

                Vector2 positionPerk = new(perk.position, perk.Level);
                perkToggle.rectTransform.anchoredPosition = perkSize * positionPerk + offset;
                perkToggle.Init_Editor(perk, this);
            }
        }
        private void CreatePerks_Ed(PerkToggle[] perks, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
                perks[i] = EUtility.InstantiatePrefab(_perkPrefab, parent);
        }
        private void DeletePerks_Ed(PerkToggle[] perks, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
                EUtility.DestroyGameObject(ref perks[i]);

            perks = parent.GetComponentsInChildren<PerkToggle>(true);
            for (int i = perks.Length - 1; i >= 0; i--)
                Object.DestroyImmediate(perks[i].gameObject);
        }
        private void CreateSpells_Ed()
        {
            SpellToggle[][] spells = { _economicSpells, _militarySpells };
            foreach (var spell in FindObjectsByType<SpellToggle>(FindObjectsSortMode.None))
                spells[spell.Type][spell.Id] = spell;
        }

        private void DeleteSeparators_Ed()
        {
            for (int i = 0; i < _countLevels; i++)
                EUtility.DestroyGameObject(ref _separators[i]);

            while (_separatorsContainer.childCount > 0)
                Object.DestroyImmediate(_separatorsContainer.GetChild(0).gameObject);
              
        }

        private void CreateSeparators_Ed()
        {
            for (int i = 0; i < _countLevels; i++)
                _separators[i] = EUtility.InstantiatePrefab(_separatorPrefab, _separatorsContainer);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = true;

            _switcher.OnValidate(this);

            this.SetChildren(ref _learnButton);
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