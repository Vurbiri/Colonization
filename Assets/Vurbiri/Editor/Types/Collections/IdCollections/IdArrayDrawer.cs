//Assets\Vurbiri\Editor\Types\Collections\IdCollections\IdArrayDrawer.cs
namespace VurbiriEditor
{
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Collections;

    [CustomPropertyDrawer(typeof(IdArray<,>))]
    public class IdArrayDrawer : ADrawerGetConstFieldName
    {
        private const int INDEX_TYPE = 0;
        protected const float Y_SPACE = 2f;
        private const string NAME_ARRAY = "_values";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                
                string[] names = GetPositiveNames(fieldInfo.FieldType.GetGenericArguments()[INDEX_TYPE]);
                int count = names.Length;
                property.arraySize = count;

                EditorGUI.indentLevel++;
                for (int i = 0; i < count; i++)
                    DrawField(propertyValues.GetArrayElementAtIndex(i), names[i]);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();

            #region Local: DrawField(...)
            //=================================
            void DrawField(SerializedProperty prop, string name)
            {
                position.y += position.height + Y_SPACE;
                EditorGUI.PropertyField(position, prop, new GUIContent(name));
                if (prop.hasVisibleChildren)
                {
                    int count = prop.Copy().CountInProperty() - 1;
                    EditorGUI.indentLevel++;
                    while (prop.NextVisible(true) && count > 0)
                    {
                        position.y += position.height + Y_SPACE;
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

            return (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
        }
    }
}
