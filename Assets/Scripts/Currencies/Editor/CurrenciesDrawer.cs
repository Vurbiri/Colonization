using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Currencies))]
public class CurrenciesDrawer : EnumArrayDrawer
{
    private const float BUTTON_RATE_POS = 1.1f, BUTTON_CLEAR_SIZE = 60f, BUTTON_PLUSMINUS_SIZE = 20f;
    private const string BUTTON_CLEAR = "Clear", BUTTON_PLUS = "+", BUTTON_MINUS = "-";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        if (property.isExpanded)
        {
            if (!Application.isPlaying)
            {
                if (DrawButton(BUTTON_CLEAR, BUTTON_CLEAR_SIZE, BUTTON_CLEAR_SIZE))
                    Clear();
                if (DrawButton(BUTTON_MINUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE))
                    Add(-1);
                if (DrawButton(BUTTON_PLUS, BUTTON_PLUSMINUS_SIZE, BUTTON_CLEAR_SIZE + BUTTON_PLUSMINUS_SIZE * 2f))
                    Add(1);
            }
        }

        #region Local: DrawButton(...), Clear(...), Add(...)
        //=================================
        bool DrawButton(string text, float size, float offset)
        {
            Rect positionButton = _position;
            float viewWidth = EditorGUIUtility.currentViewWidth;

            positionButton.height += Y_SPACE;
            positionButton.y += positionButton.height + Y_SPACE;
            positionButton.x = viewWidth - offset * BUTTON_RATE_POS;
            positionButton.width = size;

            return GUI.Button(positionButton, text.ToUpper());
        }
        //=================================
        void Clear()
        {
            for (int index = 0; index < _count; index++)
                _propertyValues.GetArrayElementAtIndex(index).intValue = 0;
        }
        //=================================
        void Add(int add)
        {
            for (int index = 0; index < _count; index++)
            {
                if ((_propertyValues.GetArrayElementAtIndex(index).intValue += add) < 0)
                    _propertyValues.GetArrayElementAtIndex(index).intValue = 0;
            }
        }
        #endregion
    }

    protected override Type GetTypeEnum() => typeof(Resource);

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float rate = 0;

        if (property.isExpanded)
            if (!Application.isPlaying)
                rate += 1.35f;

        return  base.GetPropertyHeight(property, label) + (EditorGUIUtility.singleLineHeight + Y_SPACE) * rate;
    }
}
