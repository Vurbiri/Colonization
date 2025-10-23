using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Collections;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class ScientistSettingsWindow : EditorWindow
    {
        private const string PATH = "/Colonization/Editor/Players/AI/Utility/";
        private const int COUNT = PerkTree.MAX_LEVEL, MAX = PerkTree.MAX_LEVEL - 1, PERCENT = 95;
        private const string NAME = "Scientist", MENU = MENU_CR_PATH + NAME;
         
        [SerializeField] private ScientistSettings _settings;
        [SerializeField] private PerksScriptable _perks;

        private readonly int[] _weightBase = new int[COUNT];
        private readonly int[][] _percent = { new int[EconomicPerksId.Count], new int[MilitaryPerksId.Count] };
        private readonly Perk[][,] _perkGrid = { new Perk[COUNT, COUNT], new Perk[COUNT, COUNT] };
        private readonly SerializedProperty[] _perksProperty = new SerializedProperty[AbilityTypeId.Count];
        private readonly GUIContent _chanceName = new("Chance:"), _baseName = new("Base:"), _expName = new("Exp:");

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private GUIStyle _normalStyle, _halfStyle, _overStyle;
        private SerializedObject _serializedObject;
        private SerializedProperty _chanceProperty;
        private Vector2 _scrollPos;
               
        private WeightSettings _currentSettings, _tempSettings;
        private int _maxPercent, _maxHalfPercent;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<ScientistSettingsWindow>(true, NAME);
        }

        private void OnEnable()
        {
            _normalStyle = new()
            {
                name = "WeightStyleA",
                alignment = TextAnchor.MiddleRight,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            _normalStyle.normal.textColor = new(0.4f, 0.4f, 0.8f);
            _halfStyle = new(_normalStyle);
            _halfStyle.normal.textColor = new(0.6f, 0.85f, 0.5f);
            _overStyle = new(_normalStyle);
            _overStyle.normal.textColor = new(0.95f, 0.5f, 0.1f);

            EUtility.SetScriptable(ref _perks);
            SettingsFileEditor.Load(ref _settings);
            SettingsFileEditor.Load(ref _currentSettings, PATH);
            _serializedObject = new(this);

            _tempSettings = _currentSettings;
            WeightsSetup();

            if (CheckArray(_settings.weights[AbilityTypeId.Economic]) || CheckArray(_settings.weights[AbilityTypeId.Military]))
                _settings.weights = new(new ReadOnlyArray<int>[] { new(EconomicPerksId.Count), new(MilitaryPerksId.Count) });

            var settingsProperty = _serializedObject.FindProperty(nameof(_settings));

            var weightsProperty = settingsProperty.FindPropertyRelative("weights").FindPropertyRelative("_values");
            _perksProperty[AbilityTypeId.Economic] = weightsProperty.GetArrayElementAtIndex(AbilityTypeId.Economic).FindPropertyRelative("_values");
            _perksProperty[AbilityTypeId.Military] = weightsProperty.GetArrayElementAtIndex(AbilityTypeId.Military).FindPropertyRelative("_values");

            _chanceProperty = settingsProperty.FindPropertyRelative("chance");

            PerkSetup(EconomicPerksId.Type);
            PerkSetup(MilitaryPerksId.Type);
        }

        private void WeightsSetup()
        {
            _maxPercent = _currentSettings.exp * PERCENT; _maxHalfPercent = ((_maxPercent - 100) >> 1) + 100;

            for (int i = 0; i < COUNT; i++)
                _weightBase[i] = _currentSettings.weight * MathI.Pow(_currentSettings.exp, i);
        }

        private void PerkSetup(int type)
        {
            var perks = _perks[type]; 
            var grid = _perkGrid[type];
            var percent = _percent[type];
            var perksProperty = _perksProperty[type];
            Perk perk; SerializedProperty weightProperty;
            for (int i = 0; i < AbilityTypeId.PerksCount[type]; i++)
            {
                perk = perks[i];
                grid[perk.Level, perk.position] = perk;

                weightProperty = perksProperty.GetArrayElementAtIndex(i);
                if (weightProperty.intValue == 0)
                {
                    weightProperty.intValue = _weightBase[perk.Level];
                    percent[i] = 100;
                }
                else
                {
                    percent[i] = MathI.Round(100f * weightProperty.intValue / _weightBase[perk.Level]);
                }
            }
        }

        private void OnGUI()
        {
            GUILayoutOption[] options = { GUILayout.Width(position.width * 0.975f / COUNT), GUILayout.ExpandWidth(false) };

            _serializedObject.Update();
            BeginWindows();
            {
                Space(10f);
                LabelField("Scientist Settings", STYLES.H1);
                Rect pos = BeginVertical(GUI.skin.box);
                {
                    pos.height = EditorGUIUtility.singleLineHeight; pos.width *= 0.24f; pos.x += 10f + 0.36f * position.width; pos.y += _height * 0.3f;
                    _chanceProperty.intValue = DrawSlider(pos, _chanceName, _chanceProperty.intValue, 5, 50);
                    
                    Space(_height * 1.25f);
                }
                EndVertical();
                Space();
                pos = BeginVertical(GUI.skin.box);
                {
                    float x = pos.x += 10f;
                    pos.height = EditorGUIUtility.singleLineHeight; pos.width *= 0.35f; pos.x = x + 0.14f * position.width; pos.y += _height * 0.3f;
                    _tempSettings.weight = DrawSlider(pos, _baseName, _tempSettings.weight, 50, 500);

                    pos.width = 0.24f * position.width; pos.x = x + 0.51f * position.width;
                    _tempSettings.exp = DrawSlider(pos, _expName, _tempSettings.exp, 2, 5);

                    pos.x = x + 0.36f * position.width; pos.y += _height * 1.5f;
                    if (GUI.Button(pos, "Apply"))
                    {
                        RePercent(_maxPercent, _tempSettings.exp * PERCENT);
                        _currentSettings = _tempSettings;
                        WeightsSetup();
                    }

                    Space(_height * 3f);
                }
                EndVertical();
                Space();
                BeginVertical(GUI.skin.box);
                {
                    _scrollPos = BeginScrollView(_scrollPos);
                    {
                        DrawPerks(EconomicPerksId.Type, EconomicPerksId.Names_Ed, options);
                        Space();
                        DrawPerks(MilitaryPerksId.Type, MilitaryPerksId.Names_Ed, options);
                    }
                    EndScrollView();
                }
                EndVertical();
            }
            EndWindows();
            _serializedObject.ApplyModifiedProperties();
        }

        private int DrawSlider(Rect pos, GUIContent label, int value, int min, int max)
        {
            float offset = label.text.Length * 9f;
            EditorGUI.PrefixLabel(pos, label);
            pos.x += offset; pos.width -= offset + 15f;
            return EditorGUI.IntSlider(pos, value, min, max);
        }

        private void DrawPerks(int type, string[] names, GUILayoutOption[] options)
        {
            var grid = _perkGrid[type];
            var percent = _percent[type];
            var perksProperty = _perksProperty[type];
            Perk perk; 
            int weight, ratio, id;

            BeginVertical(GUI.skin.window);
            LabelField(AbilityTypeId.Names_Ed[type], STYLES.H3);
            Space();
            for (int h = MAX; h >= 0; h--)
            {
                BeginHorizontal();
                for (int v = 0; v < COUNT; v++)
                {
                    BeginVertical(STYLES.borderLight, options);
                    {
                        perk = grid[h, v];
                        if (perk != null)
                        {
                            id = perk.Id;
                            LabelField(names[id], STYLES.H2);
                            percent[id] = ratio = IntSlider(percent[id], 100, _maxPercent);
                            perksProperty.GetArrayElementAtIndex(id).intValue = weight = _weightBase[h] * ratio / 100;
                            LabelField("WEIGHT", weight.ToString(), ratio == 100 ? _normalStyle : ratio > _maxHalfPercent ? _overStyle : _halfStyle);
                        }
                        else
                        {
                            LabelField(string.Empty, STYLES.H2);
                            LabelField(string.Empty);
                            LabelField(string.Empty);
                        }
                    }
                    EndVertical();
                }
                EndHorizontal();
            }
            Space();
            EndVertical();
        }

        private void RePercent(int oldMax,  int newMax)
        {
            float ratio = (newMax - 100f) / (oldMax - 100f);
            for (int t = 0, count; t < AbilityTypeId.Count; t++)
            {
                count = AbilityTypeId.PerksCount[t];
                var percent = _percent[t];
                for (int p = 0; p < count; p++)
                    percent[p] = MathI.Round((percent[p] - 100) * ratio) + 100;
            }    
        }

        private void OnDisable()
        {
            SettingsFileEditor.Save(_settings);
            SettingsFileEditor.Save(_currentSettings, PATH);
        }

        private static bool CheckArray(ReadOnlyArray<int> array) => array == null || array.Count == 0;

        [System.Serializable]
        private struct WeightSettings
        {
            public int weight;
            public int exp;
        }
    }
}
