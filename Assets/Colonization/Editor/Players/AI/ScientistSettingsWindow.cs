using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Characteristics;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class ScientistSettingsWindow : EditorWindow
    {
        private const int COUNT = PerkTree.MAX_LEVEL, MAX = PerkTree.MAX_LEVEL - 1, BASE = 50, EXP = 3;
        private const string NAME = "Scientist", MENU = MENU_CR_PATH + NAME;

        private static readonly int[] s_weightBase = new int[COUNT];
        private static readonly GUIStyle s_normalStyle, s_overStyle;

        [SerializeField] private ScientistSettings _settings;
        [SerializeField] private PerksScriptable _perks;

        private readonly int[][] _percent = { new int[EconomicPerksId.Count], new int[MilitaryPerksId.Count] };
        private readonly Perk[][,] _perkGrid = { new Perk[COUNT, COUNT], new Perk[COUNT, COUNT] };
        private readonly SerializedProperty[] _perksProperty = new SerializedProperty[TypeOfPerksId.Count];
        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;
        private SerializedProperty _economistProperty;
        private SerializedProperty _militaristProperty;
        private SerializedProperty _shiftMainProperty;
        private Vector2 _scrollPos;

        static ScientistSettingsWindow()
        {
            for (int i = 0; i < COUNT; i++)
                s_weightBase[i] = BASE * MathI.Pow(EXP, i);

            s_normalStyle = new()
            {
                name = "WeightStyleA",
                alignment = TextAnchor.MiddleRight,
                fontStyle = FontStyle.Bold,
                fontSize = 13,
            };
            s_normalStyle.normal.textColor = new(0.4f, 0.4f, 0.8f);
            s_overStyle = new(s_normalStyle);
            s_overStyle.normal.textColor = new(0.6f, 0.85f, 0.5f);
        }

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<ScientistSettingsWindow>(true, NAME);
        }

        private void OnEnable()
        {
            EUtility.SetScriptable(ref _perks);
            SettingsFileEditor.Load(ref _settings);
            _serializedObject = new(this);

            _serializedProperty = _serializedObject.FindProperty("_settings");
            _economistProperty = _serializedProperty.FindPropertyRelative("economist");
            _militaristProperty = _serializedProperty.FindPropertyRelative("militarist");
            _shiftMainProperty = _serializedProperty.FindPropertyRelative("shiftMain");
            _perksProperty[TypeOfPerksId.Economic] = _serializedProperty.FindPropertyRelative("economic").FindPropertyRelative("_values");
            _perksProperty[TypeOfPerksId.Military] = _serializedProperty.FindPropertyRelative("military").FindPropertyRelative("_values");

            Setup(EconomicPerksId.Type, EconomicPerksId.Count);
            Setup(MilitaryPerksId.Type, MilitaryPerksId.Count);

            _serializedObject.ApplyModifiedProperties();
        }

        private void Setup(int type, int count)
        {
            var perks = _perks[type]; 
            var grid = _perkGrid[type];
            var percent = _percent[type];
            var perksProperty = _perksProperty[type];
            Perk perk; SerializedProperty weightProperty;
            for (int i = 0; i < count; i++)
            {
                perk = perks[i];
                grid[perk.Level, perk.position] = perk;

                weightProperty = perksProperty.GetArrayElementAtIndex(i);
                if (weightProperty.intValue == 0)
                    weightProperty.intValue = s_weightBase[perk.Level];
                else
                    percent[i] = 100 * weightProperty.intValue / s_weightBase[perk.Level];
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
                    EditorGUI.PropertyField(pos, _economistProperty);
                    pos.x = x + 0.5f * position.width;
                    EditorGUI.PropertyField(pos, _militaristProperty);
                    pos.x = x + 0.26f * position.width; pos.y += _height * 2f;
                    _shiftMainProperty.intValue = EditorGUI.IntSlider(pos, _shiftMainProperty.displayName, _shiftMainProperty.intValue, 1, 5);
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


        private void DrawPerks(int type, string[] names, GUILayoutOption[] options)
        {
            var grid = _perkGrid[type];
            var percent = _percent[type];
            var perksProperty = _perksProperty[type];
            Perk perk; 
            int weight, ratio, id;

            BeginVertical(GUI.skin.window);
            LabelField(TypeOfPerksId.Names_Ed[type], STYLES.H3);
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
                            percent[id] = ratio = IntSlider(percent[id], 100, EXP * 85);
                            perksProperty.GetArrayElementAtIndex(id).intValue = weight = s_weightBase[h] * ratio / 100;
                            LabelField("WEIGHT", weight.ToString(), ratio == 100 ? s_normalStyle : s_overStyle);
                        }
                        else
                        {
                            LabelField(string.Empty, STYLES.H2);
                            LabelField(string.Empty);
                            LabelField(string.Empty, s_normalStyle);
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
    }
}
