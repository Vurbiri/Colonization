using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    public class ColorBlockDrawer
    {
        private const int COLORS_COUNT = 5;
        private static readonly GUIContent[] s_names = new GUIContent[] { new("Normal"), new("Highlighted"), new("Pressed"), new("Selected"), new("Disabled") };

        private readonly SerializedProperty _colorBlockProperty;

        private readonly SerializedProperty[] _colorProperties;
        private readonly SerializedProperty _fadeDurationProperty;
        private readonly SerializedProperty _colorMultiplierProperty;

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public ColorBlockDrawer(SerializedProperty colorBlock)
        {
            _colorBlockProperty = colorBlock;

            _colorProperties = new[]
            {
                colorBlock.FindPropertyRelative("m_NormalColor"),
                colorBlock.FindPropertyRelative("m_HighlightedColor"),
                colorBlock.FindPropertyRelative("m_PressedColor"),
                colorBlock.FindPropertyRelative("m_SelectedColor"),
                colorBlock.FindPropertyRelative("m_DisabledColor"),
            };

            _colorMultiplierProperty = colorBlock.FindPropertyRelative("m_ColorMultiplier");
            _fadeDurationProperty    = colorBlock.FindPropertyRelative("m_FadeDuration");
        }

        public void Draw()
        {
            GUIContent label = new(_colorBlockProperty.displayName);

            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;

            BeginProperty(drawRect, label, _colorBlockProperty);
            {
                if (_colorBlockProperty.isExpanded = Foldout(drawRect, _colorBlockProperty.isExpanded, label, EditorStyles.foldoutHeader))
                {
                    indentLevel++;
                    for (int i = 0; i < COLORS_COUNT; i++)
                    {
                        drawRect.y += _height;
                        PropertyField(drawRect, _colorProperties[i], s_names[i]);
                    }
                    indentLevel--;

                    drawRect.y += _height;
                    PropertyField(drawRect, _colorMultiplierProperty);
                    drawRect.y += _height;
                    FadeDurationField(drawRect, _fadeDurationProperty);
                    
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            if (_colorBlockProperty.isExpanded) EditorGUILayout.Space(8f * _height);
            else EditorGUILayout.Space(_height);
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
