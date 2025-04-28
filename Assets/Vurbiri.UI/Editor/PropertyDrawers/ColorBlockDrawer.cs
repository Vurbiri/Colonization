//Assets\Vurbiri.UI\Editor\PropertyDrawers\ColorBlockDrawer.cs
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    public class ColorBlockDrawer
    {
        private const int COUNT_SIMPLE_PROPERTIES = 6;
        private static readonly float space = EditorGUIUtility.standardVerticalSpacing;
        private static readonly float line = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private readonly SerializedProperty _colorBlockProperty;

        private readonly SerializedProperty[] _simleProperties;
        private readonly SerializedProperty _fadeDurationProperty;

        public ColorBlockDrawer(SerializedProperty colorBlock)
        {
            _colorBlockProperty      = colorBlock;

            _simleProperties = new[]
            {
                colorBlock.FindPropertyRelative("m_NormalColor"),
                colorBlock.FindPropertyRelative("m_HighlightedColor"),
                colorBlock.FindPropertyRelative("m_PressedColor"),
                colorBlock.FindPropertyRelative("m_SelectedColor"),
                colorBlock.FindPropertyRelative("m_DisabledColor"),
                colorBlock.FindPropertyRelative("m_ColorMultiplier"),
            };

            _fadeDurationProperty    = colorBlock.FindPropertyRelative("m_FadeDuration");
        }

        public void Draw()
        {
            //GUIContent label = new("Color Block");
            GUIContent label = new(_colorBlockProperty.displayName);

            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;

            BeginProperty(drawRect, label, _colorBlockProperty);
            {
                if (_colorBlockProperty.isExpanded = Foldout(drawRect, _colorBlockProperty.isExpanded, label, EditorStyles.foldoutHeader))
                {
                    for (int i = 0; i < COUNT_SIMPLE_PROPERTIES; i++)
                    {
                        drawRect.y += line;
                        PropertyField(drawRect, _simleProperties[i]);
                    }
                    drawRect.y += line;
                    FadeDurationField(drawRect, _fadeDurationProperty);
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            if (_colorBlockProperty.isExpanded) EditorGUILayout.Space(8f * line);
            else EditorGUILayout.Space(line);
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
