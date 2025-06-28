#if UNITY_EDITOR

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField, Range(5f, 20f)] private float _perkSpace = 10.5f;
        [Space]
        [SerializeField, HideInInspector] private RectTransform _economicContainer;
        [SerializeField, HideInInspector] private RectTransform _militaryContainer;
        [SerializeField, HideInInspector] private RectTransform _separatorsContainer;

        [SerializeField, HideInInspector] private PerksScriptable _perks;
        [SerializeField, HideInInspector] private ColorSettingsScriptable _colorSettings;

        [SerializeField, HideInInspector] private PerkToggle _perkPrefab;
        [SerializeField, HideInInspector] private TextMeshProUGUI _separatorPrefab;

        [SerializeField, HideInInspector] private PerkToggle[] _economic;
        [SerializeField, HideInInspector] private PerkToggle[] _military;

        [SerializeField, HideInInspector] private TextMeshProUGUI[] _separators;
#pragma warning disable 414
        [SerializeField, EndEditor] private bool _endEditor;
#pragma warning restore 414
        #endregion

        private readonly int _countSeparators = PerkTree.MAX_LEVEL + 1;

        private Vector2 PerkSize => _perkPrefab.rectTransform.sizeDelta + new Vector2(_perkSpace, _perkSpace);

        public void UpdateVisuals_Editor(float pixelsPerUnit, ProjectColors colors)
        {
            Color color = colors.PanelBack.SetAlpha(1f);
            Image image = GetComponent<Image>();
            image.color = color;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _closeButton.Color = color;
        }

        public void Setup_Editor()
        {
            SetupPerks_Editor(_economic, TypeOfPerksId.Economic, EconomicPerksId.Count, _economicContainer);
            SetupPerks_Editor(_military, TypeOfPerksId.Military, MilitaryPerksId.Count, _militaryContainer);

            float sizeX = _economicContainer.sizeDelta.x + _treeSpace;
            Vector2 mainSize = new(sizeX * 2f, _economicContainer.sizeDelta.y);
            
            GetComponent<RectTransform>().sizeDelta = mainSize + _border;

            sizeX += _treeSpace;
            _economicContainer.anchoredPosition = new(sizeX * -0.5f, 0f);
            _militaryContainer.anchoredPosition = new(sizeX * 0.5f, 0f);

            _separatorsContainer.sizeDelta = mainSize;

            Vector2 position = new(0f, PerkSize.y);
            Vector2 offset = new(0f, -_separatorsContainer.pivot.y * mainSize.y);
            SerializedObject so;;
            for (int i = 0; i < _countSeparators; i++)
            {
                _separators[i].rectTransform.anchoredPosition = position * i + offset;
                so = new(_separators[i]);
                so.FindProperty("m_text").stringValue = (i * (i + 1)).ToString();
                so.ApplyModifiedProperties();
            }
        }

        public void Create_Editor()
        {
            Delete_Editor();

            CreatePerks_Editor(_economic, EconomicPerksId.Count, _economicContainer);
            CreatePerks_Editor(_military, MilitaryPerksId.Count, _militaryContainer);

            for (int i = 0; i < _countSeparators; i++)
                _separators[i] = EUtility.InstantiatePrefab(_separatorPrefab, _separatorsContainer);

            Setup_Editor();
        }
        public void Delete_Editor()
        {
            DeletePerks_Editor(_economic, EconomicPerksId.Count);
            DeletePerks_Editor(_military, MilitaryPerksId.Count);

            for (int i = 0; i < _countSeparators; i++)
                EUtility.DestroyGameObject(ref _separators[i]);
        }

        private void SetupPerks_Editor(PerkToggle[] perks, int typePerkId, int count, RectTransform container)
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
        private void CreatePerks_Editor(PerkToggle[] perks, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
                perks[i] = EUtility.InstantiatePrefab(_perkPrefab, parent);
        }
        private void DeletePerks_Editor(PerkToggle[] perks, int count)
        {
            for (int i = 0; i < count; i++)
                EUtility.DestroyGameObject(ref perks[i]);
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = true;

            _switcher.OnValidate(this);

            this.SetChildren(ref _learnButton);
            this.SetChildren(ref _closeButton);

            _progressBars ??= new();

            this.SetChildren(ref _economicContainer, "Economic");
            this.SetChildren(ref _militaryContainer, "Military");
            this.SetChildren(ref _separatorsContainer, "Separators");

            EUtility.SetScriptable(ref _perks);
            EUtility.SetScriptable(ref _colorSettings);

            EUtility.SetPrefab(ref _perkPrefab);
            EUtility.SetPrefab(ref _separatorPrefab, "PUI_PerkSeparator");

            EUtility.SetArray(ref _economic, EconomicPerksId.Count);
            EUtility.SetArray(ref _military, MilitaryPerksId.Count);

            EUtility.SetArray(ref _separators, _countSeparators);
        }
    }
}
#endif