using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class DamageUtilityWindow : EditorWindow
    {
        private const string NAME = "Damage Utility", MENU = MENU_AC_PATH + NAME;
        private static readonly int s_count = WarriorId.Count + DemonId.Count, s_offset = WarriorId.Count;
        private static readonly string[] s_names = new string[s_count];
        private static readonly int[] s_values = new int[s_count];

        [SerializeField] WarriorsSettingsScriptable _warriorsSettings;
        [SerializeField] DemonsSettingsScriptable _demonsSettings;

        private int _attack = 20, _percent = 100, _pierce, _defense = 25;
        private DamageSkills _opponent1, _opponent2;
        private float _height;
        private Vector2 _scrollPos;

        [MenuItem(MENU)]
        public static void ShowWindow()
        {
            GetWindow<DamageUtilityWindow>(true, NAME).minSize = new(475f, 475f);
        }

        static DamageUtilityWindow()
        {
            for (int i = 0; i < s_count;  i++) s_values[i] = i;
            for (int i = 0; i < s_offset; i++) s_names[i] = string.Concat("Warrior.", WarriorId.Names_Ed[i]);
            for (int i = s_offset; i < s_count; i++) s_names[i] = string.Concat("Demon.", DemonId.Names_Ed[i - s_offset]);
        }

        private void OnEnable()
        {
            _opponent1 = new(this, 0);
            _opponent2 = new(this, s_offset);
            OpponentsUpdate();
        }

        private void OnGUI()
        {
            BeginWindows();
            {
                Space(10f);
                LabelField(NAME, STYLES.H1);

                BeginVertical(GUI.skin.box);
                {
                    _scrollPos = BeginScrollView(_scrollPos);
                    {
                        BeginVertical(STYLES.borderDark);
                        {
                            _attack = IntSlider("Attack", _attack, 10, 100);
                            _pierce = IntSlider("Pierce", _pierce, 0, 50);
                            Space(1f);
                            _percent = IntSlider("Percent", _percent, 50, 350);
                            Space(3f);
                            _defense = IntSlider("Defense", _defense, 1, 100);
                            Space();

                            BeginVertical(STYLES.borderLight);
                            IntSlider("Damage", Formulas.Damage(_attack * _percent / 100, _defense * (100 - _pierce) / 100), 0, 100);
                            EndVertical();
                            Space(1f);
                        }
                        EndVertical();

                        Space();

                        Rect position = BeginVertical(STYLES.borderDark);
                        {
                            float center = position.width * 0.5f, offset = position.x;
                            position.height = EditorGUIUtility.singleLineHeight; position.width = center - 20f; position.y += 3f; position.x += 5f;

                            EditorGUI.BeginChangeCheck();
                            {
                                _opponent1.DrawId(position);
                                position.x += center + 10f;
                                _opponent2.DrawId(position);

                            }
                            if (EditorGUI.EndChangeCheck())
                                OpponentsUpdate();

                            position.x = offset + center - 15f; position.width = 30f;
                            EditorGUI.LabelField(position, "VS", STYLES.H3);

                            position.x = offset + center - 0.5f; position.width = 1f;
                            position.y += EditorGUIUtility.singleLineHeight + 5f; position.height = _height;
                            EditorGUI.DrawRect(position, Color.red);

                            Space(EditorGUIUtility.singleLineHeight);

                            BeginHorizontal();
                            {
                                BeginVertical(GUILayout.Width(center));
                                    _opponent1.DrawSkills(false);
                                EndVertical();
                                BeginVertical(GUILayout.Width(center));
                                    _opponent2.DrawSkills(true);
                                EndVertical();
                            }
                            EndHorizontal();

                            if (GUILayout.Button("UPDATE"))
                                OpponentsUpdate();
                        }
                        EndVertical();
                    }
                    EndScrollView();
                }
                EndVertical();
            }
            EndWindows();
        }

        private void OpponentsUpdate()
        {
            _opponent1.Update();
            _opponent2.Update();

            bool isHoly = _opponent1.IsDemon ^ _opponent2.IsDemon;
            _opponent1.Calk(_opponent2.Defense, isHoly);
            _opponent2.Calk(_opponent1.Defense, isHoly);

            _height = MathF.Max(_opponent1.GetHeight(), _opponent2.GetHeight());
        }

        // *********************** Nested *************************************
        private class DamageSkills
        {
            private readonly WarriorsSettingsScriptable _warriorsSettings;
            private readonly DemonsSettingsScriptable _demonsSettings;
            private List<SkillHits_Ed> _skills;
            private int _id;
            private string _params;
            private int _attack, _pierce, _defense;

            public bool IsDemon => _id >= s_offset;
            public int Defense => _defense;

            public DamageSkills(DamageUtilityWindow parent, int id)
            {
                _warriorsSettings = parent._warriorsSettings;
                _demonsSettings = parent._demonsSettings;
                _id = id;
            }

            public void DrawId(Rect position) => _id = EditorGUI.IntPopup(position, _id, s_names, s_values);

            public void DrawSkills(bool offset)
            {
                if (offset) ++EditorGUI.indentLevel;
                LabelField(_params);
                Space(3f);
                ++EditorGUI.indentLevel;
                for (int i = 0; i < _skills.Count; ++i)
                {
                    _skills[i].Draw();
                    Space(3f);
                }
                if (offset) --EditorGUI.indentLevel; --EditorGUI.indentLevel;
                Space(3f);
            }

            public void Update()
            {
                ActorSettings settings = IsDemon ? _demonsSettings.Settings[_id - s_offset] : _warriorsSettings.Settings[_id];

                (_attack, _pierce, _defense) = settings.GetAttackPierceDefence_Ed();
                _skills = settings.Skills.GetSkillHits_Ed();

                _params = $"Attack: {_attack}. Pierce: {_pierce}. Defense: {_defense}.";
            }

            public void Calk(int defense, bool isHoly)
            {
                for (int i = 0; i < _skills.Count; ++i)
                    _skills[i].Update(_attack, _pierce, defense, isHoly);
            }

            public float GetHeight()
            {
                float height = _skills.Count + 1f;
                for (int i = 0; i < _skills.Count; ++i)
                    height += _skills[i].Count;
                height *= EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                height += (_skills.Count + 3) * 3f;
                
                return height;
            }
        }
    }
}