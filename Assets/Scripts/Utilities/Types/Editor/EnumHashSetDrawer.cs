using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumHashSet<,>))]
public class EnumHashSetDrawer : PropertyDrawer
{
    private const float Y_SPACE = 2f, BUTTON_SIZE = 125f, LABEL_SIZE = 100f;
    private const string NAME_PROPERTY = "_values", LABEL_EMPTY = "-----";
    private const string BUTTON_CHILD = "Set children", BUTTON_PREF = "Set prefabs", BUTTON_CLEAR = "Clear";
    private static readonly Color colorNull = new(1f, 0.65f, 0f, 1f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        position.y += Y_SPACE;

        Color prevColor = GUI.color;
        Rect startPosition = position;
        Type typeKey = fieldInfo.FieldType.GetGenericArguments()[0], typeValue = fieldInfo.FieldType.GetGenericArguments()[1];
        SerializedProperty propertyValues = property.FindPropertyRelative(NAME_PROPERTY);
        int countCurrent = 0, count = propertyValues.arraySize;
        
        EditorGUI.BeginProperty(position, label, property);

        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            SerializedProperty propertyCurrent, propertyNull = null;
            string[] Names = Enum.GetNames(fieldInfo.FieldType.GetGenericArguments()[0]);

            EditorGUI.indentLevel++;
            for (int i = 0; i < count; i++)
            {
                propertyCurrent = propertyValues.GetArrayElementAtIndex(i);
                if (propertyCurrent.objectReferenceValue != null)
                {
                    countCurrent++;
                    DrawField(propertyCurrent, Names[i]);
                    continue;
                }
                
                propertyNull = propertyCurrent;
            }
            if (propertyNull != null)
                DrawField(propertyNull, LABEL_EMPTY);
            EditorGUI.indentLevel--;

            if (!Application.isPlaying && typeValue.Is(typeof(Component)))
            {
                if (property.serializedObject.targetObject is MonoBehaviour && DrawButton(BUTTON_CHILD))
                    GetComponentsInChildren(property.serializedObject.targetObject as MonoBehaviour);
                if (DrawButton(BUTTON_PREF))
                    LoadAssets();
            }
            if (DrawButton(BUTTON_CLEAR))
                Clear();
        }
        else
        {
            for (int i = 0; i < count; i++)
                if (propertyValues.GetArrayElementAtIndex(i).objectReferenceValue != null)
                    countCurrent++;
        }
        DrawLabelCount();

        EditorGUI.EndProperty();

        #region Local: DrawField(...), DrawLabelCount(), DrawButton(), SetPropertyArray(...)
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
        void DrawLabelCount()
        {
            GUIStyle style = new(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight
            };

            startPosition.x = EditorGUIUtility.currentViewWidth - LABEL_SIZE - 20f;
            startPosition.width = LABEL_SIZE;

            if (countCurrent < count)
                GUI.color = colorNull;
            EditorGUI.LabelField(startPosition, $"{countCurrent} / {count}", style);
            GUI.color = prevColor;
        }
        //=================================
        bool DrawButton(string text)
        {
            position.height += Y_SPACE;
            position.y += position.height + Y_SPACE * 2f;
            position.x = (EditorGUIUtility.currentViewWidth - BUTTON_SIZE) * 0.5f;
            position.width = BUTTON_SIZE;
            return GUI.Button(position, text.ToUpper());
        }
        //=================================
        void GetComponentsInChildren(MonoBehaviour mono)
        {
            UnityEngine.Object[] array = mono.GetComponentsInChildren(fieldInfo.FieldType.GetGenericArguments()[1]);

            SetValues(array);
        }
        //=================================
        void LoadAssets()
        {
            string[] guids = AssetDatabase.FindAssets($"t:Prefab", new[] { "Assets" });
            string path;
            UnityEngine.Object obj;
            List<UnityEngine.Object> list = new();

            foreach (var guid in guids)
            {
                path = AssetDatabase.GUIDToAssetPath(guid);
                obj = (AssetDatabase.LoadMainAssetAtPath(path) as GameObject).GetComponent(typeValue);
                if (obj != null) 
                    list.Add(obj);
            }

            SetValues(list);
        }
        //=================================
        void SetValues( IReadOnlyList<UnityEngine.Object> array)
        {
            for (int index = 0; index < array.Count; index++)
                propertyValues.GetArrayElementAtIndex(index % count).objectReferenceValue = array[index];
        }
        //=================================
        void Clear()
        {
            for (int index = 0; index < count; index++)
                propertyValues.GetArrayElementAtIndex(index).objectReferenceValue = null;
        }
        #endregion

    }

    private const float BUTTON_RATE = 1.3f;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float rate = 1.01f;
        int countNull = 0;
        if (property.isExpanded)
        {
            SerializedProperty propertyValues = property.FindPropertyRelative(NAME_PROPERTY);
            for (int i = 0; i < propertyValues.arraySize; i++)
            {
                if (propertyValues.GetArrayElementAtIndex(i).objectReferenceValue != null)
                    rate += 1f;
                else if (countNull == 0)
                    countNull++;
            }

            Type typeValue = fieldInfo.FieldType.GetGenericArguments()[1];
            if (!Application.isPlaying && typeValue.Is(typeof(Component)))
            {
                if (property.serializedObject.targetObject is MonoBehaviour)
                    rate += BUTTON_RATE;
                rate += BUTTON_RATE;
            }
            rate += BUTTON_RATE;
        }

        return (EditorGUIUtility.singleLineHeight + Y_SPACE) * (rate + countNull);
    }
}
