using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumArray<,>))]
public class EnumArrayDrawer : PropertyDrawer
{
    private const float Y_SPACE = 2f;
    private const string NAME_PROPERTY = "_values", TEXT_BUTTON = "Set children";
    private static readonly Color colorNull = new(1f, 0.65f, 0f, 1f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Color prevColor = GUI.color;
        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.BeginProperty(position, label, property);

        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            SerializedProperty propertyValues = property.FindPropertyRelative(NAME_PROPERTY);
            string[] Names = Enum.GetNames(fieldInfo.FieldType.GetGenericArguments()[0]);

            EditorGUI.indentLevel++;
            for (int i = 0; i < propertyValues.arraySize; i++)
                DrawField(propertyValues.GetArrayElementAtIndex(i), Names[i]);
            EditorGUI.indentLevel--;

            if(IsComponent() && property.serializedObject.targetObject is MonoBehaviour && DrawButton())
                SetPropertyArray(propertyValues, property.serializedObject.targetObject as MonoBehaviour);
        }

        EditorGUI.EndProperty();

        #region Local: DrawField(...), DrawButton(), SetPropertyArray(...)
        //=================================
        void DrawField(SerializedProperty prop, string name)
        {
            position.y += position.height + Y_SPACE;
            if (prop.objectReferenceValue == null)
                GUI.color = colorNull;
            EditorGUI.PropertyField(position, prop, new GUIContent(name));
            GUI.color = prevColor;
        }
        //=================================
        bool DrawButton()
        {
            position.height += Y_SPACE;
            position.y += position.height + Y_SPACE * 2f;
            position.x = position.width * 0.4f;
            position.width = TEXT_BUTTON.Length * 8.5f;
            return GUI.Button(position, TEXT_BUTTON.ToUpper());
        }
        void SetPropertyArray(SerializedProperty property, MonoBehaviour mono)
        {
            UnityEngine.Object[] array = mono.GetComponentsInChildren(fieldInfo.FieldType.GetGenericArguments()[1]);

            for (int index = 0; index < array.Length; index++)
                property.GetArrayElementAtIndex(index % property.arraySize).objectReferenceValue = array[index];
        }
        #endregion

    }

    private bool IsComponent()
    {
        Type typeValue = fieldInfo.FieldType.GetGenericArguments()[1];
        Type typeComponent = typeof(UnityEngine.Component);

        while (typeValue != null)
        {
            if (typeValue == typeComponent)
                return true;

            typeValue = typeValue.BaseType;
        }

        return false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float count = 1f;
        if (property.isExpanded)
        {
            count += property.FindPropertyRelative(NAME_PROPERTY).arraySize;
            if (IsComponent() && property.serializedObject.targetObject is MonoBehaviour)
                count += 1.3f;
        }

        return (EditorGUIUtility.singleLineHeight + Y_SPACE) * count;
    }
}
