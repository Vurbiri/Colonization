using System;
using UnityEditor;
using UnityEngine;

public abstract class AGetComponentAttributeDrawer : PropertyDrawer
{
    private static Color colorNull = new(1f, 0.6f, 0f, 1f);

    private static bool isEnable = true;
    private const string MENU_NAME = "CONTEXT/Component/AutoGetComponent";
    [MenuItem(MENU_NAME)]
    private static void SetActive()
    {
        isEnable = !isEnable;
        Menu.SetChecked(MENU_NAME, isEnable);
    }
    static AGetComponentAttributeDrawer() => Menu.SetChecked(MENU_NAME, isEnable);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type typeProperty = fieldInfo.FieldType;
        Color prevColor = GUI.color;

        property.serializedObject.Update();

        if (isEnable && !Application.isPlaying && !CheckPropertyValue())
        {
            MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;
            SetProperty(property, mono.gameObject, typeProperty);
        }

        if (!CheckPropertyValue())
            GUI.color = colorNull;

        EditorGUI.PropertyField(position, property, label);

        property.serializedObject.ApplyModifiedProperties();
        GUI.color = prevColor;

        #region Local: CheckPropertyValue()
        //=================================
        bool CheckPropertyValue()
        {
            if (property.objectReferenceValue == null)
                return false;
            
            Type typeValue = property.objectReferenceValue.GetType();
            while (typeValue != null)
            {
                if (typeValue == typeProperty)
                    return true;

                typeValue = typeValue.BaseType;
            }

            return false;
        }
        #endregion
    }

    protected abstract void SetProperty(SerializedProperty property, GameObject gameObject, Type type);
}
