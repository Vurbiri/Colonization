//Assets\Vurbiri\Editor\Types\Collections\IdCollections\IdArrayDrawer.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Collections;

namespace VurbiriEditor.Collections
{
    [CustomPropertyDrawer(typeof(IdArray<,>))]
    public class IdArrayDrawer : AIdCollectionDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);
            {
                if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
                {
                    SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);

                    GetPositiveNames();
                    int count = _names.Length;
                    propertyValues.arraySize = count;

                    EditorGUI.indentLevel++;
                    for (int i = 0; i < count; i++)
                        DrawField(propertyValues.GetArrayElementAtIndex(i), _names[i]);
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUI.EndProperty();

            #region Local: DrawField(...)
            //=================================
            void DrawField(SerializedProperty prop, string name)
            {
                position.y += _height;
                EditorGUI.PropertyField(position, prop, new GUIContent(name));
                if (prop.hasVisibleChildren)
                {
                    int count = prop.Copy().CountInProperty() - 1;
                    EditorGUI.indentLevel++;
                    while (prop.NextVisible(true) && count > 0)
                    {
                        position.y += _height;
                        EditorGUI.PropertyField(position, prop, new GUIContent(prop.displayName));
                        count--;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            //=================================
            #endregion
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float rate = 1.01f;

            if (property.isExpanded)
            {
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                int count = propertyValues.arraySize;
                for (int i = 0; i < count; i++)
                    rate += propertyValues.GetArrayElementAtIndex(i).CountInProperty();
            }

            return _height * rate;
        }
    }
}
