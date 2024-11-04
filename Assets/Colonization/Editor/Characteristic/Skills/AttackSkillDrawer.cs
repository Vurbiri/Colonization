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
        private const string PROP_REM_T = "remainingTime", PROP_DAMAGE_T = "damageTime", PROP_RANGE = "range", PROP_SPEED = "moveSpeed", PROP_ID_A = "idAnimation";
        private const string PROP_DAMAGE = "percentDamage", PROP_COST = "cost";
        private const string PROP_SPRITE = "sprite", PROP_NAME_K = "keyName", PROP_DESK_K = "keyDesc";

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
                   
                    SerializedProperty parentProperty = property.FindPropertyRelative(PROP_SETTINGS);
                    SerializedProperty damageProperty = parentProperty.FindPropertyRelative(PROP_DAMAGE);
                    SerializedProperty costProperty = parentProperty.FindPropertyRelative(PROP_COST);

                    EditorGUI.indentLevel++;
                    position.y += height;
                    EditorGUI.LabelField(position, "Total Time", $"{clipSett.totalTime}");
                    DrawLabelAndSetValue(parentProperty, PROP_DAMAGE_T, clipSett.damageTime);
                    DrawLabelAndSetValue(parentProperty, PROP_REM_T, clipSett.totalTime - clipSett.damageTime);
                    DrawLabelAndSetValue(parentProperty, PROP_RANGE, clipSett.range);
                    EditorGUI.indentLevel--;

                    position.y += ySpace * 2f;
                    if (DrawFoldout(parentProperty))
                    {
                        EditorGUI.indentLevel++;

                        position.y += height;
                        damageProperty.floatValue = EditorGUI.Slider(position, damageProperty.displayName, damageProperty.floatValue, 0f, 100f);

                        DrawIntSlider(parentProperty, PROP_ID_A, 2);

                        position.y += height;
                        costProperty.intValue = EditorGUI.IntSlider(position, costProperty.displayName, costProperty.intValue, 0, 3);

                        DrawSlider(parentProperty, PROP_SPEED, 1f);

                        EditorGUI.indentLevel--;
                    }

                    parentProperty = property.FindPropertyRelative(PROP_UI);
                    parentProperty.FindPropertyRelative(PROP_DAMAGE).floatValue = damageProperty.floatValue;
                    parentProperty.FindPropertyRelative(PROP_COST).intValue = costProperty.intValue;

                    position.y += ySpace;
                    if (DrawFoldout(parentProperty))
                    {
                        EditorGUI.indentLevel++;

                        DrawObject<Sprite>(parentProperty, PROP_SPRITE, true);
                        DrawString(parentProperty, PROP_NAME_K);
                        DrawString(parentProperty, PROP_DESK_K);

                        EditorGUI.indentLevel--;
                    }

                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();

            #region Local: DrawLabelAndSetValue(..), DrawFoldout(..), DrawObject<T>(..), DrawString(..), DrawSlider(..), DrawIntSlider(..), DrawButton()
            //=================================
            void DrawLabelAndSetValue(SerializedProperty parent, string name, float value)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.floatValue = value;
                EditorGUI.LabelField(position, $"{property.displayName}", $"{value}");
            }
            //=================================
            bool DrawFoldout(SerializedProperty property)
            {
                position.y += height;
                return property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, property.displayName.ToUpper());
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
            void DrawString(SerializedProperty parent, string name)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.stringValue = EditorGUI.TextField(position, property.displayName, property.stringValue);
            }
            //=================================
            void DrawSlider(SerializedProperty parent, string name, float max)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.floatValue = EditorGUI.Slider(position, property.displayName, property.floatValue, 0, max);
            }
            //=================================
            void DrawIntSlider(SerializedProperty parent, string name, int max)
            {
                position.y += height;
                SerializedProperty property = parent.FindPropertyRelative(name);
                property.intValue = EditorGUI.IntSlider(position, property.displayName, property.intValue, 0, max);
            }
            //=================================
            void DrawButton(Object activeObject)
            {
                position.y += height;
                Rect positionButton = position;
                float viewWidth = EditorGUIUtility.currentViewWidth;

                positionButton.height += ySpace * 2f;
                positionButton.x = 75f;
                positionButton.width = viewWidth - 115f;

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
            float rate = 1.1f;

            if (property.isExpanded)
            {
                rate *= 2.1f;
                AnimationClipSettingsScriptable clipSett = property.FindPropertyRelative(PROP_CLIP).objectReferenceValue as AnimationClipSettingsScriptable;
                if (clipSett != null && clipSett.clip != null)
                {
                    rate += 7.2f;
                    if (property.FindPropertyRelative(PROP_SETTINGS).isExpanded)
                        rate += 4.1f;
                    if (property.FindPropertyRelative(PROP_UI).isExpanded)
                        rate += 3.1f;
                }
            }

            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * rate;
        }
    }
}
