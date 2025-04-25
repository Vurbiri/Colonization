//Assets\Vurbiri.UI\Editor\PropertyDrawers\ColorBlockDrawer.cs
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    public class ColorBlockDrawer
    {
        private static readonly float space = EditorGUIUtility.standardVerticalSpacing;
        private static readonly float line = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private readonly SerializedProperty _colorBlock;

        private readonly SerializedProperty _normalColor;
        private readonly SerializedProperty _highlighted;
        private readonly SerializedProperty _pressedColor;
        private readonly SerializedProperty _selectedColor;
        private readonly SerializedProperty _disabledColor;
        private readonly SerializedProperty _colorMultiplier;
        private readonly SerializedProperty _fadeDuration;

        public ColorBlockDrawer(SerializedProperty colorBlock)
        {
            _colorBlock = colorBlock;

            _normalColor = colorBlock.FindPropertyRelative("m_NormalColor");
            _highlighted = colorBlock.FindPropertyRelative("m_HighlightedColor");
            _pressedColor = colorBlock.FindPropertyRelative("m_PressedColor");
            _selectedColor = colorBlock.FindPropertyRelative("m_SelectedColor");
            _disabledColor = colorBlock.FindPropertyRelative("m_DisabledColor");
            _colorMultiplier = colorBlock.FindPropertyRelative("m_ColorMultiplier");
            _fadeDuration = colorBlock.FindPropertyRelative("m_FadeDuration");
        }

        public void OnGUI()
        {
            //GUIContent label = new("Color Block");
            GUIContent label = new(_colorBlock.displayName);

            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;
            drawRect.y -= space;

            BeginProperty(drawRect, label, _colorBlock);
            {
                if (_colorBlock.isExpanded = Foldout(drawRect, _colorBlock.isExpanded, label, EditorStyles.foldoutHeader))
                {
                    drawRect.y += line;
                    PropertyField(drawRect, _normalColor);
                    drawRect.y += line;
                    PropertyField(drawRect, _highlighted);
                    drawRect.y += line;
                    PropertyField(drawRect, _pressedColor);
                    drawRect.y += line;
                    PropertyField(drawRect, _selectedColor);
                    drawRect.y += line;
                    PropertyField(drawRect, _disabledColor);
                    drawRect.y += line + space;
                    PropertyField(drawRect, _colorMultiplier);
                    drawRect.y += line;
                    FadeDurationField(drawRect, _fadeDuration);
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            if (_colorBlock.isExpanded) EditorGUILayout.Space(8f * line);
            else EditorGUILayout.Space(line - space);
        }

        private static void FadeDurationField(Rect position, SerializedProperty property)
        {
            GUIContent label = new(property.displayName);
            BeginProperty(position, label, property);
            {
                property.floatValue = Slider(position, label, property.floatValue, 0f, 1f);
            }
            EndProperty();
        }
    }
}
