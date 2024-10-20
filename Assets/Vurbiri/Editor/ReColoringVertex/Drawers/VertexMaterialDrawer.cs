using UnityEditor;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor.ReColoringVertex
{
    [CustomPropertyDrawer(typeof(VertexMaterial))]
    internal class VertexMaterialDrawer : PropertyDrawer
    {
        private const string NAME_NAME = "name", NAME_MESH = "colorMesh", NAME_REPLACE = "colorReplace", NAME_SPECULAR = "specular";
        private const string NAME_MODE = "isInfoMode", NAME_EDITING = "isEditValue", NAME_EDIT_NAME = "isEditName", NAME_OPEN = "isOpen";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;
            float stepHeight = EditorGUIUtility.singleLineHeight + space;

            bool isInfoMode = property.FindPropertyRelative(NAME_MODE).boolValue;
            bool isEditValue = property.FindPropertyRelative(NAME_EDITING).boolValue;
            bool isEditName = property.FindPropertyRelative(NAME_EDIT_NAME).boolValue;

            SerializedProperty isOpenProperty = property.FindPropertyRelative(NAME_OPEN);

            SerializedProperty nameProperty = property.FindPropertyRelative(NAME_NAME);
            SerializedProperty colorMeshProperty = property.FindPropertyRelative(NAME_MESH);
            SerializedProperty colorReplaceProperty = property.FindPropertyRelative(NAME_REPLACE);
            SerializedProperty vectorProperty = property.FindPropertyRelative(NAME_SPECULAR);
            
            Vector2 vector = vectorProperty.vector2Value;

            label = EditorGUI.BeginProperty(position, label, property);

            if (isEditName)
            {
                Rect textPos = position;
                if(!isOpenProperty.boolValue) textPos.width = EditorGUIUtility.labelWidth;
                EditorGUI.indentLevel += isInfoMode ? 0 : 1;
                nameProperty.stringValue = EditorGUI.TextField(textPos, nameProperty.stringValue);
                EditorGUI.indentLevel -= isInfoMode ? 0 : 1;
            }

            if (isOpenProperty.boolValue = EditorGUI.Foldout(position, isOpenProperty.boolValue, isEditName ? "" : nameProperty.stringValue, false))
            {
                EditorGUI.indentLevel++;

                if (!isInfoMode)
                {
                    position.y += stepHeight + space * 0.5f;
                    EditorGUI.ColorField(position, labelEmpty, colorMeshProperty.colorValue, false, false, false);
                    position.y += space;
                }

                if (isInfoMode || isEditValue)
                {
                    position.y += stepHeight + space;

                    colorReplaceProperty.colorValue = EditorGUI.ColorField(position, labelColor, colorReplaceProperty.colorValue, true, true, false);

                    for (int i = 0; i < COUNT_SPECULAR; i++)
                        DrawSpecular(i);
                    vectorProperty.vector2Value = vector;
                }

                EditorGUI.indentLevel--;
            }
            else
            {
                position.width -= EditorGUIUtility.labelWidth;
                position.x += EditorGUIUtility.labelWidth;

                EditorGUI.ColorField(position, labelEmpty, isInfoMode ? colorReplaceProperty.colorValue : colorMeshProperty.colorValue, false, false, false);
                
            }

            EditorGUI.EndProperty();

            #region Local: DrawSpecular(...)
            //=================================
            void DrawSpecular(int id)
            {
                position.y += stepHeight;
                vector[id] = EditorGUI.Slider(position, labelsSpecular[id], vector[id], 0f, 1f);
            }
            //=================================
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool isInfoMode = property.FindPropertyRelative(NAME_MODE).boolValue;
            bool isEditValue = property.FindPropertyRelative(NAME_EDITING).boolValue;
            bool isOpen = property.FindPropertyRelative(NAME_OPEN).boolValue;

            float space = EditorGUIUtility.standardVerticalSpacing;
            float height = EditorGUIUtility.singleLineHeight + space;
            float size = 0f;

            if (isOpen)
            {
                if (!isInfoMode)
                    size += height + space * 1.5f;

                if (isInfoMode || isEditValue)
                    size += (height + space) * 3f;
            }

            size += height + space;

            return size;
        }

    }
}
