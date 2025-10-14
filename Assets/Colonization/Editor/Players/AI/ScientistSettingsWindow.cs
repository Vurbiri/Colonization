using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Collections;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class ScientistSettingsWindow : EditorWindow
    {
        private const int COUNT = PerkTree.MAX_LEVEL, MAX = PerkTree.MAX_LEVEL - 1, BASE = 50, EXP = 3, MAX_PERCENT = EXP * 90, HALF_MAX_PERCENT = ((MAX_PERCENT - 100) >> 1) + 100;
        private const string NAME = "Scientist", MENU = MENU_CR_PATH + NAME;
         
        [SerializeField] private ScientistSettings _settings;
        [SerializeField] private PerksScriptable _perks;

        private readonly int[] _weightBase = new int[COUNT];
        private readonly int[][] _percent = { new int[EconomicPerksId.Count], new int[MilitaryPerksId.Count] };
        private readonly Perk[][,] _perkGrid = { new Perk[COUNT, COUNT], new Perk[COUNT, COUNT] };
        private readonly SerializedProperty[] _perksProperty = new SerializedProperty[AbilityTypeId.Count];
        private readonly SerializedProperty[] _specProperty = new SerializedProperty[AbilityTypeId.Count];
        private readonly GUIContent[] _specName = { new("Economist"), new("Militarist") };

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private GUIStyle _normalStyle, _halfStyle, _overStyle;
        private SerializedObject _serializedObject;
        private SerializedProperty _shiftProperty;
        private Vector2 _scrollPos;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<ScientistSettingsWindow>(true, NAME);
        }

        private void OnEnable()
        {
            for (int i = 0; i < COUNT; i++)
                _weightBase[i] = BASE * MathI.Pow(EXP, i);

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
            _serializedObject = new(this);

            if (CheckArray(_settings.weights[AbilityTypeId.Economic]) || CheckArray(_settings.weights[AbilityTypeId.Military]))
                _settings.weights = new(new ReadOnlyArray<int>[] { new(EconomicPerksId.Count), new(MilitaryPerksId.Count) });

            var serializedProperty = _serializedObject.FindProperty("_settings");

            var weightsProperty = serializedProperty.FindPropertyRelative("weights").FindPropertyRelative("_values");
            _perksProperty[AbilityTypeId.Economic] = weightsProperty.GetArrayElementAtIndex(AbilityTypeId.Economic).FindPropertyRelative("_values");
            _perksProperty[AbilityTypeId.Military] = weightsProperty.GetArrayElementAtIndex(AbilityTypeId.Military).FindPropertyRelative("_values");

            var specializationProperty = serializedProperty.FindPropertyRelative("specialization").FindPropertyRelative("_values");
            _specProperty[AbilityTypeId.Economic] = specializationProperty.GetArrayElementAtIndex(AbilityTypeId.Economic);
            _specProperty[AbilityTypeId.Military] = specializationProperty.GetArrayElementAtIndex(AbilityTypeId.Military);

            _shiftProperty = serializedProperty.FindPropertyRelative("shift");

            Setup(EconomicPerksId.Type);
            Setup(MilitaryPerksId.Type);
        }

        private void Setup(int type)
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
                    weightProperty.intValue = _weightBase[perk.Level];
                else
                    percent[i] = 100 * weightProperty.intValue / _weightBase[perk.Level];
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
                Space(20f);
                Rect pos = BeginVertical();
                {
                    float x = pos.x += 10f;
                    pos.height = EditorGUIUtility.singleLineHeight; pos.width *= 0.45f;
                    DrawSpecialization(pos, AbilityTypeId.Economic);
                    pos.x = x + 0.5f * position.width;
                    DrawSpecialization(pos, AbilityTypeId.Military);
                    pos.x = x + 0.26f * position.width; pos.y += _height * 2f;
                    _shiftProperty.intValue = EditorGUI.IntSlider(pos, _shiftProperty.displayName, _shiftProperty.intValue, 1, 5);
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

        private void DrawSpecialization(Rect pos, int type) => EditorGUI.PropertyField(pos, _specProperty[type], _specName[type]);

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
                            percent[id] = ratio = IntSlider(percent[id], 100, MAX_PERCENT);
                            perksProperty.GetArrayElementAtIndex(id).intValue = weight = _weightBase[h] * ratio / 100;
                            LabelField("WEIGHT", weight.ToString(), ratio == 100 ? _normalStyle : ratio > HALF_MAX_PERCENT ? _overStyle : _halfStyle);
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

        private void OnDisable()
        {
            SettingsFileEditor.Save(_settings);
        }

        private static bool CheckArray(ReadOnlyArray<int> array) => array == null || array.Count == 0;
    }
}
