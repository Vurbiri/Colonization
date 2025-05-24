using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    public class ScaleBlockDrawer
	{
        private const int COLORS_COUNT = 5, AXIS_COUNT = 3;

        private static readonly GUIContent s_buttonOn, s_buttonOff;

        private readonly SerializedProperty _scaleBlockProperty;

        private readonly SerializedProperty[] _scaleProperties;
        private readonly SerializedProperty[] _modeProperties;
        private readonly SerializedProperty _fadeDurationProperty;

        private readonly float _height = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;

        static ScaleBlockDrawer()
        {
            Texture image = Resources.Load<Texture>("Icons/d_Linked");
            s_buttonOn = new(image, "Disable all axis editing");

            image = Resources.Load<Texture>("Icons/d_Unlinked");
            s_buttonOff = new(image, "Enable all axis editing");
        }

        public ScaleBlockDrawer(SerializedProperty scaleBlock)
        {
            _scaleBlockProperty   = scaleBlock;

            _scaleProperties = new[]
            {
                scaleBlock.FindPropertyRelative("normal"),
                scaleBlock.FindPropertyRelative("highlighted"),
                scaleBlock.FindPropertyRelative("pressed"),
                scaleBlock.FindPropertyRelative("selected"),
                scaleBlock.FindPropertyRelative("disabled"),
            };
            _modeProperties = new[]
            {
                scaleBlock.FindPropertyRelative("_normalMode"),
                scaleBlock.FindPropertyRelative("_highlightedMode"),
                scaleBlock.FindPropertyRelative("_pressedMode"),
                scaleBlock.FindPropertyRelative("_selectedMode"),
                scaleBlock.FindPropertyRelative("_disabledMode"),
            };

            _fadeDurationProperty = scaleBlock.FindPropertyRelative("fadeDuration");
        }

        public void Draw(bool editFade)
        {
            float drawLine = 6f;
            GUIContent label = new(_scaleBlockProperty.displayName);

            Rect drawRect = EditorGUILayout.BeginVertical();
            drawRect.height = EditorGUIUtility.singleLineHeight;

            BeginProperty(drawRect, label, _scaleBlockProperty);
            {
                if (_scaleBlockProperty.isExpanded = Foldout(drawRect, _scaleBlockProperty.isExpanded, label, EditorStyles.foldoutHeader))
                {
                    indentLevel++;
                    for (int i = 0; i < COLORS_COUNT; i++)
                    {
                        drawRect.y += _height;
                        VectorField(drawRect, _scaleProperties[i], _modeProperties[i]);
                    }
                    indentLevel--;

                    if(editFade)
                    { 
                        drawRect.y += _height; drawLine += 1f;
                        FadeDurationField(drawRect, _fadeDurationProperty);
                    }
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            if (_scaleBlockProperty.isExpanded) EditorGUILayout.Space(drawLine * _height);
            else EditorGUILayout.Space(_height);
        }

        private static void VectorField(Rect position, SerializedProperty vector, SerializedProperty editMode)
        {
            bool mode = editMode.boolValue;
            Vector3 oldScale = vector.vector3Value;

            if (GUI.Button(GetButtonRect(position), mode ? s_buttonOff : s_buttonOn, STYLES.flatButton))
            {
                if (mode)
                {
                    float value = (oldScale.x + oldScale.y) * 0.5f;
                    vector.vector3Value = oldScale = new(value, value, value);
                }
                editMode.boolValue = mode = !mode;
            }

            if(mode)
            {
                PropertyField(position, vector);
                return;
            }

            BeginChangeCheck();
                PropertyField(position, vector);
            if (EndChangeCheck())
            {
                Vector3 newScale = vector.vector3Value;
                for (int i = 0; i < AXIS_COUNT; i++)
                {
                    if (oldScale[i] != newScale[i])
                    {
                        float value = newScale[i];
                        vector.vector3Value = new(value, value, value);
                        return;
                    }
                }
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

        private static Rect GetButtonRect(Rect position)
        {
            Rect sizeButton = position;
            sizeButton.width = EditorGUIUtility.singleLineHeight;
            sizeButton.x = EditorGUIUtility.labelWidth;
            return sizeButton;
        }
    }
}
