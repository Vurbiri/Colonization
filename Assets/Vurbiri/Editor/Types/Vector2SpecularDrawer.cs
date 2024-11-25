//Assets\Vurbiri\Editor\ReColoringVertex\Drawers\Vector2SpecularDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    [CustomPropertyDrawer(typeof(Vector2Specular))]
    public class Vector2SpecularDrawer : PropertyDrawer
    {
        private const string NAME_VALUE = "_value";
        private static readonly string[] labelsSpecular = { "Metallic", "Smoothness" };
        private const int COUNT_SPECULAR = 2;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            float y_space = EditorGUIUtility.standardVerticalSpacing;
            SerializedProperty vectorProperty = property.FindPropertyRelative(NAME_VALUE);
            Vector2 vector = vectorProperty.vector2Value;

            label = EditorGUI.BeginProperty(position, label, property);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < COUNT_SPECULAR; i++)
                    DrawField(i);
                EditorGUI.indentLevel--;

                vectorProperty.vector2Value = vector;
            }

            EditorGUI.EndProperty();

            #region Local: DrawField(...)
            //=================================
            void DrawField(int id)
            {
                position.y += position.height + y_space;
                vector[id] = EditorGUI.Slider(position, labelsSpecular[id], vector[id], 0f, 1f);
            }
            //=================================
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
                height += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2f;

            return height;
        }
    }
}
