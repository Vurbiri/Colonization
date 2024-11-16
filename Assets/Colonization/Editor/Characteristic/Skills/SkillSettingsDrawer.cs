namespace VurbiriEditor.Colonization.Characteristics
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Colonization.Actors;
    using Vurbiri.Colonization.Characteristics;

    [CustomPropertyDrawer(typeof(Skills.SkillSettings))]
    public class SkillSettingsDrawer : PropertyDrawerUtility
    {
        private const string NAME_ELEMENT = "Skill {0}";
        private const string P_CLIP = "clipSettings", P_MOVE = "isMove", P_VALID = "isValid", P_SETTINGS = "settings", P_UI = "ui";
        private const string P_REM_T = "remainingTime", P_DAMAGE_T = "damageTime", P_RANGE = "range", P_ID_A = "idAnimation";
        private const string P_TARGET = "target", P_COST = "cost", P_EFFECTS = "effects";
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
                var clipSett = DrawObject<AnimationClipSettingsScriptable>(P_CLIP);

                bool isValid = clipSett != null && clipSett.clip != null;
                mainProperty.FindPropertyRelative(P_VALID).boolValue = isValid;

                if (isValid)
                {
                    DrawButton(clipSett);
                   
                    SerializedProperty settingsProperty = mainProperty.FindPropertyRelative(P_SETTINGS);
                    SerializedProperty costProperty = settingsProperty.FindPropertyRelative(P_COST);
                    SerializedProperty uiProperty = mainProperty.FindPropertyRelative(P_UI);

                    DrawLine(Color.gray);
                    EditorGUI.indentLevel++;
                    _position.y += _height;
                    EditorGUI.LabelField(_position, "Total Time", $"{clipSett.totalTime}");
                    DrawLabelAndSetValue(settingsProperty, P_DAMAGE_T, clipSett.damageTime);
                    DrawLabelAndSetValue(settingsProperty, P_REM_T, clipSett.totalTime - clipSett.damageTime);
                    DrawLabelAndSetValue(P_RANGE, clipSett.range);
                    DrawLabelAndSetValue(settingsProperty, P_ID_A, id);
                    EditorGUI.indentLevel--;
                    DrawLine(Color.gray);

                    if (DrawId(P_TARGET, typeof(TargetOfEffectId)) != TargetOfEffectId.Self)
                    {
                        Space();
                        DrawBool(P_MOVE);
                    }
                    Space();
                    DrawSelfIntSlider(costProperty, 0, 3);

                    
                    uiProperty.FindPropertyRelative(P_COST).intValue = costProperty.intValue;

                    Space(2f);
                    DrawStringPopup(uiProperty, P_KEY_NAME, KEYS_NAME_SKILLS);
                    DrawObject<Sprite>(uiProperty, P_SPRITE, true);

                    Space(2f); _position.y += _height;
                    EditorGUI.PropertyField(_position, mainProperty.FindPropertyRelative(P_EFFECTS));
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();

            #region Local: DrawSelfIntSlider(..), DrawButton(...)
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
                    rate += 12.6f;
                    if (property.FindPropertyRelative(P_TARGET).intValue != TargetOfEffectId.Self)
                        rate += 1.2f;

                    SerializedProperty effectsProperty = property.FindPropertyRelative(P_EFFECTS);
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