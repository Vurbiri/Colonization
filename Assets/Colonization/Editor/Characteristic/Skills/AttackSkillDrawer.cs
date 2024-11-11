using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(Skills.AttackSkill))]
    public class AttackSkillDrawer : PropertyDrawer
    {
        private const string NAME_ELEMENT = "Attack {0}";
        private const string PROP_CLIP = "clipSettings", PROP_VALID = "isValid", PROP_SETTINGS = "settings", PROP_UI = "ui";
        private const string PROP_REM_T = "remainingTime", PROP_DAMAGE_T = "damageTime", PROP_RANGE = "range", PROP_ID_A = "idAnimation";
        private const string PROP_DAMAGE = "percentDamage", PROP_COST = "cost", PROP_EFFECTS = "effects";
        private const string PROP_SPRITE = "sprite", PROP_KEY_NAME = "keyName";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            position.width *= 0.95f;

            float ySpace = EditorGUIUtility.standardVerticalSpacing;
            float height = EditorGUIUtility.singleLineHeight + ySpace;

            string[] strings = label.text.Split(' ');
            if (strings.Length == 2)
                label.text = string.Format(NAME_ELEMENT, strings[1]);

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel++;

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                EditorGUI.indentLevel++;

                position.y += ySpace;
                SerializedProperty clipProperty = DrawObject<AnimationClipSettingsScriptable>(property, PROP_CLIP);
                AnimationClipSettingsScriptable clipSett = clipProperty.objectReferenceValue as AnimationClipSettingsScriptable;

                bool isValid = clipSett != null && clipSett.clip != null;
                property.FindPropertyRelative(PROP_VALID).boolValue = isValid;

                if (isValid)
                {
                    DrawButton(clipSett);
                   
                    SerializedProperty settingsProperty = property.FindPropertyRelative(PROP_SETTINGS);
                    SerializedProperty damageProperty = property.FindPropertyRelative(PROP_DAMAGE);
                    SerializedProperty costProperty = settingsProperty.FindPropertyRelative(PROP_COST);

                    EditorGUI.indentLevel++;
                    position.y += height;
                    EditorGUI.LabelField(position, "Total Time", $"{clipSett.totalTime}");
                    DrawLabelAndSetValue(settingsProperty, PROP_DAMAGE_T, clipSett.damageTime);
                    DrawLabelAndSetValue(settingsProperty, PROP_REM_T, clipSett.totalTime - clipSett.damageTime);
                    DrawLabelAndSetValue(settingsProperty, PROP_RANGE, clipSett.range);
                    EditorGUI.indentLevel--;

                    position.y += height + ySpace;
                    damageProperty.intValue = EditorGUI.IntSlider(position, damageProperty.displayName, damageProperty.intValue, 50, 250);

                    position.y += ySpace * 2f;
                    DrawIntSlider(settingsProperty, PROP_ID_A, 2);

                    position.y += height;
                    costProperty.intValue = EditorGUI.IntSlider(position, costProperty.displayName, costProperty.intValue, 0, 3);

                    SerializedProperty uiProperty = property.FindPropertyRelative(PROP_UI);
                    uiProperty.FindPropertyRelative(PROP_DAMAGE).intValue = damageProperty.intValue;
                    uiProperty.FindPropertyRelative(PROP_COST).intValue = costProperty.intValue;

                    position.y += ySpace * 2f;
                    DrawString(uiProperty, PROP_KEY_NAME);
                    DrawObject<Sprite>(uiProperty, PROP_SPRITE, true);

                    position.y += height + ySpace * 2f;
                    EditorGUI.PropertyField(position, settingsProperty.FindPropertyRelative(PROP_EFFECTS));
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();

            #region Local: DrawLabelAndSetValue(..), DrawObject<T>(..), DrawIntSlider(..), DrawButton()
            //=================================
            void DrawLabelAndSetValue(SerializedProperty parent, string name, float value)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.floatValue = value;
                EditorGUI.LabelField(position, $"{property.displayName}", $"{value}");
            }
            //=================================
            SerializedProperty DrawObject<T>(SerializedProperty parent, string name, bool isName = false)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                if (isName)
                {
                    property.objectReferenceValue = EditorGUI.ObjectField(position, property.displayName, property.objectReferenceValue, typeof(T), false);
                    return property;
                }

                property.objectReferenceValue = EditorGUI.ObjectField(position, property.objectReferenceValue, typeof(T), false);
                return property;
            }
            //=================================
            void DrawIntSlider(SerializedProperty parent, string name, int max)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.intValue = EditorGUI.IntSlider(position, property.displayName, property.intValue, 0, max);
            }
            //=================================
            void DrawString(SerializedProperty parent, string name)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.stringValue = EditorGUI.TextField(position, property.displayName, property.stringValue);
            }
            //=================================
            void DrawButton(Object activeObject)
            {
                position.y += height;
                Rect positionButton = position;
                float viewWidth = EditorGUIUtility.currentViewWidth;

                positionButton.height += ySpace * 2f;
                positionButton.x = 100f;
                positionButton.width = viewWidth - 125f;

                if (GUI.Button(positionButton, "Select Clip Settings".ToUpper()))
                {
                    //AnimationClipSettingsWindow.ShowWindow();
                    Selection.activeObject = activeObject;
                }

                position.y += ySpace * 2f;
            }
            //=================================
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1f;

            if (property.isExpanded)
            {
                rate *= 2.1f;
                AnimationClipSettingsScriptable clipSett = property.FindPropertyRelative(PROP_CLIP).objectReferenceValue as AnimationClipSettingsScriptable;
                if (clipSett != null && clipSett.clip != null)
                {
                    SerializedProperty parentProperty = property.FindPropertyRelative(PROP_SETTINGS);
                    SerializedProperty effectsProperty = parentProperty.FindPropertyRelative(PROP_EFFECTS);
                    rate += 12.6f;
                    if(effectsProperty.isExpanded)
                    {
                        rate += 2.1f;
                        rate += 6.6f * effectsProperty.arraySize;
                    }
                }
            }

            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * rate;
        }
    }
}
