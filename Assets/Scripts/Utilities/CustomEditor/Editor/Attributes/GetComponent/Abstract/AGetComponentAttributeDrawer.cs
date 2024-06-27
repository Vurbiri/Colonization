using System;
using UnityEditor;
using UnityEngine;

public abstract class AGetComponentAttributeDrawer : PropertyDrawer
{
    protected static Color colorNull = new(1f, 0.6f, 0f, 1f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type typeProperty = fieldInfo.FieldType;
        Color prevColor = GUI.color;

        if (!Application.isPlaying && IsPropertySet(property, typeProperty))
        {
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            SetProperty(property, mono.gameObject, typeProperty);
        }

        if (IsPropertyError(property, typeProperty))
        {
            GUI.color = colorNull;
            //Debug.LogWarningFormat(property.serializedObject.targetObject, $"Объекту <b>{property.serializedObject.targetObject.name}</b> не назначено поле <b>{fieldInfo.Name}</b> ({typeProperty.Name}).");
        }

        EditorGUI.PropertyField(position, property, label);

        GUI.color = prevColor;
    }

    protected virtual bool IsPropertySet(SerializedProperty property, Type typeProperty) => !IsValidValue(property.objectReferenceValue, typeProperty);
    protected virtual bool IsPropertyError(SerializedProperty property, Type typeProperty) => !IsValidValue(property.objectReferenceValue, typeProperty);

    protected virtual bool IsValidValue(UnityEngine.Object value, Type typeProperty)
    {
        if (value == null)
            return false;

        return value.GetType().Is(typeProperty);
    }

    protected abstract void SetProperty(SerializedProperty property, GameObject gameObject, Type type);


}
