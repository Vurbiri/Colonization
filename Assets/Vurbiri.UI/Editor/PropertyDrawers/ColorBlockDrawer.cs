//Assets\Vurbiri.UI\Editor\PropertyDrawers\ColorBlockDrawer.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.UI
{
    public class ColorBlockDrawer
    {
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

            _normalColor     = colorBlock.FindPropertyRelative("m_NormalColor");
            _highlighted     = colorBlock.FindPropertyRelative("m_HighlightedColor");
            _pressedColor    = colorBlock.FindPropertyRelative("m_PressedColor");
            _selectedColor   = colorBlock.FindPropertyRelative("m_SelectedColor");
            _disabledColor   = colorBlock.FindPropertyRelative("m_DisabledColor");
            _colorMultiplier = colorBlock.FindPropertyRelative("m_ColorMultiplier");
            _fadeDuration    = colorBlock.FindPropertyRelative("m_FadeDuration");
        }

        public void OnGUI()
        {
            GUIContent label = new(_colorBlock.displayName);
            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;
            drawRect.y -= EditorGUIUtility.singleLineHeight * 0.5f;

            EditorGUI.BeginProperty(drawRect, label, _colorBlock);
            {
                if (_colorBlock.isExpanded = EditorGUI.Foldout(drawRect, _colorBlock.isExpanded, label, EditorStyles.foldoutHeader))
                {
                    drawRect.y += line;
                    EditorGUI.PropertyField(drawRect, _normalColor);
                    drawRect.y += line;
                    EditorGUI.PropertyField(drawRect, _highlighted);
                    drawRect.y += line;
                    EditorGUI.PropertyField(drawRect, _pressedColor);
                    drawRect.y += line;
                    EditorGUI.PropertyField(drawRect, _selectedColor);
                    drawRect.y += line;
                    EditorGUI.PropertyField(drawRect, _disabledColor);
                    drawRect.y += line;
                    EditorGUI.PropertyField(drawRect, _colorMultiplier);
                    drawRect.y += line;
                    _fadeDuration.floatValue =  EditorGUI.Slider(drawRect, _fadeDuration.displayName ,_fadeDuration.floatValue, 0f, 2f);
                }
            }
            EditorGUI.EndProperty();
            EditorGUILayout.EndVertical();

            if (_colorBlock.isExpanded) EditorGUILayout.Space(7.6f * line);
            else EditorGUILayout.Space(0.6f * line);
        }
    }
}
