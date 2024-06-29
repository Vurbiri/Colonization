using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Currencies))]
public class CurrenciesDrawer : PropertyDrawer
{
    private const float Y_SPACE = 2f, BUTTON_RATE_POS = 1.1f, BUTTON_CLEAR_SIZE = 60f, BUTTON_PLUSMINUS_SIZE = 20f;
    private const string NAME_ARRAY = "_values", BUTTON_CLEAR = "Clear", BUTTON_PLUS = "+", BUTTON_MINUS = "-";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.BeginProperty(position, label, property);

        if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
        {
            SerializedProperty propertyValues = property.FindPropertyRelative(NAME_ARRAY);
            int count = propertyValues.arraySize;
            string[] names = Enum.GetNames(typeof(Resource));

            EditorGUI.indentLevel++;
            for (int i = 0; i < count; i++)
                DrawField(propertyValues.GetArrayElementAtIndex(i), names[i]);
            EditorGUI.indentLevel--;

            if (!Application.isPlaying)
            {
                if (DrawButton(BUTTON_CLEAR, BUTTON_CLEAR_SIZE, BUTTON_CLEAR_SIZE))
                    Clear(propertyValues, count);
                if (DrawButton(BUTTON_MINUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE))
                    Add(propertyValues, count, -1);
                if (DrawButton(BUTTON_PLUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE * 2f))
                    Add(propertyValues, count, 1);
            }
        }

        EditorGUI.EndProperty();

        #region Local: DrawField(...), DrawButton(...), Clear(...), Add(...)
        //=================================
        void DrawField(SerializedProperty prop, string name)
        {
            position.y += position.height + Y_SPACE;
            EditorGUI.PropertyField(position, prop, new GUIContent(name));
        }
        //=================================
        //=================================
        bool DrawButton(string text, float size, float offset)
        {
            Rect positionButton = position;
            float viewWidth = EditorGUIUtility.currentViewWidth;

            positionButton.height += Y_SPACE;
            positionButton.y += positionButton.height + Y_SPACE;
            positionButton.x = viewWidth - offset * BUTTON_RATE_POS;
            positionButton.width = size;

            return GUI.Button(positionButton, text.ToUpper());
        }
        //=================================
        void Clear(SerializedProperty prop, int c)
        {
            for (int index = 0; index < c; index++)
                prop.GetArrayElementAtIndex(index).intValue = 0;
        }
        //=================================
        void Add(SerializedProperty prop, int c, int add)
        {
            for (int index = 0; index < c; index++)
            {
                if ((prop.GetArrayElementAtIndex(index).intValue += add) < 0)
                    prop.GetArrayElementAtIndex(index).intValue = 0;
            }
        }

        #endregion
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float rate = 1.01f;

        if (property.isExpanded)
        {
            rate += property.FindPropertyRelative(NAME_ARRAY).arraySize;
            if (!Application.isPlaying)
                rate += 1.35f;
        }


        return (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
    }
}
