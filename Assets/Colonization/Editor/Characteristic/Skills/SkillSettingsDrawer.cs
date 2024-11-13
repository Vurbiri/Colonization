namespace VurbiriEditor.Colonization
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Colonization;
    using Vurbiri.Colonization.Actors;

    [CustomPropertyDrawer(typeof(Skills.SkillSettings))]
    public class SkillSettingsDrawer : PropertyDrawerUtility
    {
        private const string NAME_ELEMENT = "Skill {0}";
        private const string P_CLIP = "clipSettings", P_MOVE = "isMove", P_VALID = "isValid", P_SETTINGS = "settings", P_UI = "ui";
        private const string P_REM_T = "remainingTime", P_DAMAGE_T = "damageTime", P_RANGE = "range", P_ID_A = "idAnimation";
        private const string P_DAMAGE = "percentDamage", P_COST = "cost", P_EFFECTS = "effects";
        private const string P_SPRITE = "sprite", P_KEY_NAME = "keyName";
        private readonly string[] KEYS_NAME_SKILLS = { "Attack", "Sweep" };

        public override void OnGUI(Rect mainPosition, SerializedProperty mainProperty, GUIContent label)
        {
            base.OnGUI(mainPosition, mainProperty, label);

            string[] strings = label.text.Split(' ');
            int id = -1;
            if (strings.Length == 2)
            {
                id = int.Parse(strings[1]);
                label.text = string.Format(NAME_ELEMENT, id);
            }

            label = EditorGUI.BeginProperty(_position, label, mainProperty);
            EditorGUI.indentLevel++;

            if (mainProperty.isExpanded = EditorGUI.Foldout(_position, mainProperty.isExpanded, label))
            {
                EditorGUI.indentLevel++;

                Space();
                SerializedProperty clipProperty = DrawObject<AnimationClipSettingsScriptable>(mainProperty, P_CLIP);
                AnimationClipSettingsScriptable clipSett = clipProperty.objectReferenceValue as AnimationClipSettingsScriptable;

                bool isValid = clipSett != null && clipSett.clip != null;
                mainProperty.FindPropertyRelative(P_VALID).boolValue = isValid;

                if (isValid)
                {
                    DrawButton(clipSett);
                   
                    SerializedProperty settingsProperty = mainProperty.FindPropertyRelative(P_SETTINGS);
                    SerializedProperty damageProperty = mainProperty.FindPropertyRelative(P_DAMAGE);
                    SerializedProperty costProperty = settingsProperty.FindPropertyRelative(P_COST);

                    DrawLine(Color.gray);
                    EditorGUI.indentLevel++;
                    _position.y += _height;
                    EditorGUI.LabelField(_position, "Total Time", $"{clipSett.totalTime}");
                    DrawLabelAndSetValue(settingsProperty, P_DAMAGE_T, clipSett.damageTime);
                    DrawLabelAndSetValue(settingsProperty, P_REM_T, clipSett.totalTime - clipSett.damageTime);
                    DrawLabelAndSetValue(mainProperty, P_RANGE, clipSett.range);
                    DrawLabelAndSetValue(settingsProperty, P_ID_A, id);
                    EditorGUI.indentLevel--;
                    DrawLine(Color.gray);

                    DrawBool(P_MOVE);
                    DrawSelfIntSlider(damageProperty, 50, 250);

                    Space(2f);
                    DrawSelfIntSlider(costProperty, 0, 3);

                    SerializedProperty uiProperty = mainProperty.FindPropertyRelative(P_UI);
                    uiProperty.FindPropertyRelative(P_DAMAGE).intValue = damageProperty.intValue;
                    uiProperty.FindPropertyRelative(P_COST).intValue = costProperty.intValue;

                    Space(2f);
                    DrawStringPopup(uiProperty, P_KEY_NAME, KEYS_NAME_SKILLS);
                    DrawObject<Sprite>(uiProperty, P_SPRITE, true);

                    Space(2f); _position.y += _height;
                    EditorGUI.PropertyField(_position, settingsProperty.FindPropertyRelative(P_EFFECTS));
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();

            #region Local: DrawLabelAndSetValue<T>(..), DrawSelfIntSlider(..), DrawButton(...)
            //=================================
            void DrawLabelAndSetValue<T>(SerializedProperty parent, string name, T value)
            {
                _position.y += _height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                if(value is float fValue)
                    property.floatValue = fValue;
                else if(value is int iValue)
                    property.intValue = iValue;
                EditorGUI.LabelField(_position, $"{property.displayName}", $"{value}");
            }

            //=================================
            void DrawSelfIntSlider(SerializedProperty property, int min, int max)
            {
                _position.y += _height;
                property.intValue = EditorGUI.IntSlider(_position, property.displayName, property.intValue, min, max);
            }
            //=================================
            void DrawButton(UnityEngine.Object activeObject)
            {
                _position.y += _height;
                Rect positionButton = _position;
                float viewWidth = EditorGUIUtility.currentViewWidth;

                positionButton.height += _ySpace * 2f;
                positionButton.x = 100f;
                positionButton.width = viewWidth - 125f;

                if (GUI.Button(positionButton, "Select Clip Settings".ToUpper()))
                {
                    //AnimationClipSettingsWindow.ShowWindow();
                    Selection.activeObject = activeObject;
                }

                _position.y += _ySpace * 2f;
            }
            //=================================
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1f;

            if (property.isExpanded)
            {
                rate *= 2.5f;
                AnimationClipSettingsScriptable clipSett = property.FindPropertyRelative(P_CLIP).objectReferenceValue as AnimationClipSettingsScriptable;
                if (clipSett != null && clipSett.clip != null)
                {
                    SerializedProperty parentProperty = property.FindPropertyRelative(P_SETTINGS);
                    SerializedProperty effectsProperty = parentProperty.FindPropertyRelative(P_EFFECTS);
                    rate += 13.7f;
                    if(effectsProperty.isExpanded)
                    {
                        rate += 2.4f;
                        for (int i = 0; i < effectsProperty.arraySize; i++)
                            rate += EffectSettingsDrawer.GetPropertyRateHeight(effectsProperty.GetArrayElementAtIndex(i));
                    }
                }
            }

            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * rate;
        }
    }
}
