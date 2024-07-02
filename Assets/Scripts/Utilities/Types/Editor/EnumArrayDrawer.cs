using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumArray<,>))]
public class EnumArrayDrawer : PropertyDrawer
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

        EditorGUI.BeginProperty(position, label, property);

        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            _propertyValues = property.FindPropertyRelative(NAME_ARRAY);
            _count = _propertyValues.arraySize;
            string[] names = Enum.GetNames(GetTypeEnum());

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
        }
        //=================================
        #endregion
    }

    protected virtual Type GetTypeEnum() => fieldInfo.FieldType.GetGenericArguments()[INDEX_TYPE];

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float rate = 1.01f;

        if (property.isExpanded)
            rate += property.FindPropertyRelative(NAME_ARRAY).arraySize;

        return (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
    }
}
