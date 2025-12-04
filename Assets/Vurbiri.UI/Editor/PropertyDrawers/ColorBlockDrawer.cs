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
        //private readonly SerializedProperty _colorMultiplierProperty;

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

            _fadeDurationProperty    = colorBlock.FindPropertyRelative("m_FadeDuration");
            //_colorMultiplierProperty = colorBlock.FindPropertyRelative("m_ColorMultiplier");
        }

        public void Draw()
        {
            GUIContent label = new(_colorBlockProperty.displayName);

            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;
            drawRect.y += EditorGUIUtility.standardVerticalSpacing;

            FadeDurationField(drawRect, _fadeDurationProperty);
            drawRect.y += EditorGUIUtility.singleLineHeight;

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
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(2f * _height - EditorGUIUtility.standardVerticalSpacing);
            if (_colorBlockProperty.isExpanded) 
                EditorGUILayout.Space(5f * _height);
            
        }

        public void DrawGUILayout()
        {
            EditorGUILayout.Slider(_fadeDurationProperty, 0f, 1f);
            EditorGUILayout.Space();

            if (_colorBlockProperty.isExpanded = EditorGUILayout.Foldout(_colorBlockProperty.isExpanded, _colorBlockProperty.displayName))
            {
                indentLevel++;
                for (int i = 0; i < COLORS_COUNT; i++)
                    EditorGUILayout.PropertyField(_colorProperties[i], s_names[i]);
                indentLevel--;
            }
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
