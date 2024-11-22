//Assets\Vurbiri\Editor\Types\Collections\IdCollections\IdArrayDrawer.cs
namespace VurbiriEditor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using Vurbiri.Collections;

    [CustomPropertyDrawer(typeof(IdArray<,>))]
    public class IdArrayDrawer : ADrawerGetConstFieldName
    {
        private const int INDEX_TYPE = 0;
        protected const float Y_SPACE = 2f;
        private const string NAME_ARRAY = "_values";

        protected Rect _position;
        protected SerializedProperty _propertyValues;
        protected int _count;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            label = EditorGUI.BeginProperty(position, label, property);

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
            {
                _propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                _count = _propertyValues.arraySize;
                List<string> names = GetNames(fieldInfo.FieldType.GetGenericArguments()[INDEX_TYPE]);

                EditorGUI.indentLevel++;
                for (int i = 0; i < _count; i++)
                    DrawField(_propertyValues.GetArrayElementAtIndex(i), names[i]);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();

            _position = position;

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
                _propertyValues = property.FindPropertyRelative(NAME_ARRAY);
                _count = _propertyValues.arraySize;
                for (int i = 0; i < _count; i++)
                    rate += _propertyValues.GetArrayElementAtIndex(i).CountInProperty();
            }

            return (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
        }
    }
}
