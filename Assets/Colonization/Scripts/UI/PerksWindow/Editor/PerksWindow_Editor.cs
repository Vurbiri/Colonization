#if UNITY_EDITOR



using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
	public partial class PerksWindow
    {
        [StartEditor]
        [SerializeField, Range(2f, 10f)] private float _border = 5f;
        [SerializeField, Range(20f, 60f)] private float _treeSpace = 40f;
        [Space]
        [SerializeField] private Vector2 _perkSpace = new(11f, 11f);
        [Space]
        [SerializeField] private RectTransform _economicContainer;
        [SerializeField] private RectTransform _militaryContainer;
        [Space]
        [SerializeField, ReadOnly] private PerksScriptable _perks;
        [SerializeField, ReadOnly] private ColorSettingsScriptable _colorSettings;
        [SerializeField, ReadOnly] private PerkToggle _prefab;
        [SerializeField, HideInInspector] private PerkToggle[] _economic = new PerkToggle[EconomicPerksId.Count];
        [SerializeField, HideInInspector] private PerkToggle[] _military = new PerkToggle[MilitaryPerksId.Count];
#pragma warning disable 414
        [SerializeField, EndEditor] private bool _endEditor;
#pragma warning restore 414

        protected override void OnValidate()
        {
            base.OnValidate();

            _allowSwitchOff = true;

            EUtility.SetChildren(ref _learnButton, this, "LearnButton");

            EUtility.SetChildren(ref _economicContainer, this, "Economic");
            EUtility.SetChildren(ref _militaryContainer, this, "Military");

            EUtility.SetScriptable(ref _perks);
            EUtility.SetScriptable(ref _colorSettings);
            EUtility.SetPrefab(ref _prefab);
            EUtility.SetArray(ref _economic, EconomicPerksId.Count);
            EUtility.SetArray(ref _military, MilitaryPerksId.Count);
        }
        public void Setup_Editor()
        {
            Setup_Editor(_economic, TypeOfPerksId.Economic, EconomicPerksId.Count, _economicContainer);
            Setup_Editor(_military, TypeOfPerksId.Military, MilitaryPerksId.Count, _militaryContainer);

            float sizeX = _economicContainer.sizeDelta.x + _treeSpace;
            ((RectTransform)transform).sizeDelta = new(sizeX * 2f + _border, _economicContainer.sizeDelta.y + _border);

            sizeX += _treeSpace;
            _economicContainer.anchoredPosition = new(sizeX * -0.5f, 0f);
            _militaryContainer.anchoredPosition = new(sizeX * 0.5f, 0f);

            GetComponent<Image>().color = _colorSettings.Colors.PanelBack;
        }

        public void Create_Editor()
        {
            Delete_Editor(_economic, EconomicPerksId.Count);
            Delete_Editor(_military, MilitaryPerksId.Count);

            Create_Editor(_economic, EconomicPerksId.Count, _economicContainer);
            Create_Editor(_military, MilitaryPerksId.Count, _militaryContainer);

            Setup_Editor();
        }
        public void Delete_Editor()
        {
            Delete_Editor(_economic, EconomicPerksId.Count);
            Delete_Editor(_military, MilitaryPerksId.Count);
        }
        public void Setup_Editor(PerkToggle[] perks, int typePerkId, int count, RectTransform container)
        {
            Vector2 perkSize = perks[0].rectTransform.sizeDelta + _perkSpace;
            container.sizeDelta = perkSize * (PerkTree.MAX_LEVEL + 1) + _perkSpace * 0.5f;

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
        private void Create_Editor(PerkToggle[] perks, int count, Transform parent)
        {
            for (int i = 0; i < count; i++)
            {
                if (perks[i] == null)
                {
                    perks[i] = (PerkToggle)UnityEditor.PrefabUtility.InstantiatePrefab(_prefab);
                    perks[i].rectTransform.SetParent(parent, false);
                }
            }
        }
        public void Delete_Editor(PerkToggle[] perks, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if(perks[i] != null)
                    DestroyImmediate(perks[i].gameObject);
                perks[i] = null;
            }
        }
    }
}
#endif