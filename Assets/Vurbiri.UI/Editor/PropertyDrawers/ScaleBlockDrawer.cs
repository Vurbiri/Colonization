//Assets\Vurbiri.UI\Editor\PropertyDrawers\ScaleBlockDrawer.cs
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    public class ScaleBlockDrawer
	{
        private const int COUNT_COLORS = 6;
        private static readonly float space = EditorGUIUtility.standardVerticalSpacing;
        private static readonly float line = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private readonly SerializedProperty _scaleBlockProperty;

        private readonly SerializedProperty[] _colorProperties;
        private readonly SerializedProperty _fadeDurationProperty;

        public ScaleBlockDrawer(SerializedProperty colorBlock)
        {
            _scaleBlockProperty   = colorBlock;

            _colorProperties = new[]
            {
                colorBlock.FindPropertyRelative("normal"),
                colorBlock.FindPropertyRelative("highlighted"),
                colorBlock.FindPropertyRelative("pressed"),
                colorBlock.FindPropertyRelative("selected"),
                colorBlock.FindPropertyRelative("disabled"),
            };

            _fadeDurationProperty = colorBlock.FindPropertyRelative("fadeDuration");
        }

        public void Draw()
        {
            GUIContent label = new(_scaleBlockProperty.displayName);

            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;

            BeginProperty(drawRect, label, _scaleBlockProperty);
            {
                if (_scaleBlockProperty.isExpanded = Foldout(drawRect, _scaleBlockProperty.isExpanded, label, EditorStyles.foldoutHeader))
                {
                    for (int i = 0; i < COUNT_COLORS; i++)
                    {
                        drawRect.y += line;
                        PropertyField(drawRect, _colorProperties[i]);
                    }
                    drawRect.y += line;
                    FadeDurationField(drawRect, _fadeDurationProperty);
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            if (_scaleBlockProperty.isExpanded) EditorGUILayout.Space(7f * line);
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
