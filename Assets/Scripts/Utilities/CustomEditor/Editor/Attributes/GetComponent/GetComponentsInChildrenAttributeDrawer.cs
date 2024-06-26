using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GetComponentsInChildrenAttribute))]
public class GetComponentsInChildrenAttributeDrawer : AGetComponentAttributeDrawer
{
    private GetComponentsInChildrenAttribute _attribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Color prevColor = GUI.color;
        Type typeProperty = fieldInfo.FieldType;

        if (typeProperty.IsArray || typeProperty.GetInterface(nameof(IList)) != null)
        {
            _attribute = attribute as GetComponentsInChildrenAttribute;

            SerializedProperty propArray = property.serializedObject.FindProperty(fieldInfo.Name);
            typeProperty = typeProperty.IsArray ? typeProperty.GetElementType() : typeProperty.GetGenericArguments()[_attribute.IndexGeneric];

            if (!Application.isPlaying && IsPropertySet(propArray, typeProperty))
            {
                MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
                SetProperty(propArray, mono.gameObject, typeProperty);
            }

            if (IsPropertyError(propArray, typeProperty))
                GUI.color = colorNull;

            EditorGUI.PropertyField(position, property, label);
        }
        else
        {
            GUI.color = Color.red;

            EditorGUILayout.PropertyField(property, label);
            EditorGUILayout.HelpBox("Not array", UnityEditor.MessageType.Error);
        }
        
        GUI.color = prevColor;
    }

    protected override bool IsPropertySet(SerializedProperty property, Type typeProperty)
    {
        for (int i = 0; i < property.arraySize; i++)
            if (IsValidValue(property.GetArrayElementAtIndex(i).objectReferenceValue, typeProperty))
                return false;

        return true;
    }

    protected override bool IsPropertyError(SerializedProperty property, Type typeProperty)
    {
        for (int i = 0; i < property.arraySize; i++)
            if (!IsValidValue(property.GetArrayElementAtIndex(i).objectReferenceValue, typeProperty))
                return true;

        return false;
    }

    protected override void SetProperty(SerializedProperty property, GameObject gameObject, Type type)
    {
        SetPropertyArray(property, gameObject.GetComponentsInChildren(type, _attribute.IncludeInactive));
    }

    protected void SetPropertyArray(SerializedProperty property, UnityEngine.Object[] array)
    {
        int count = array.Length;

        while (property.arraySize < count)
            property.InsertArrayElementAtIndex(property.arraySize);

        for (int index = 0; index < count; index++)
            property.GetArrayElementAtIndex(index).objectReferenceValue = array[index];

        while (property.arraySize > count)
            property.DeleteArrayElementAtIndex(count);
    }


    
}
