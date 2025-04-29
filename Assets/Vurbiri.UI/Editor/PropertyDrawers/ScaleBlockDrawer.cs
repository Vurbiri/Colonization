//Assets\Vurbiri.UI\Editor\PropertyDrawers\ScaleBlockDrawer.cs
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.UI
{
    public class ScaleBlockDrawer
	{
        #region Const
        private const string PATH_ON_IMAGE = "Icons/d_Linked", PATH_OFF_IMAGE = "Icons/d_Unlinked";
        private const string TOOLTIP_ON_BUTTON = "Disable all axis editing", TOOLTIP_OFF_BUTTON = "Enable all axis editing";

        private const int COLORS_COUNT = 5, AXIS_COUNT = 3;
        #endregion
        #region Static
        private static readonly GUIContent buttonOn, buttonOff;
        //private static readonly GUIStyle flatButtonStyle;

        private static readonly float space, height, line;
        #endregion

        private readonly SerializedProperty _scaleBlockProperty;

        private readonly SerializedProperty[] _colorProperties;
        private readonly SerializedProperty[] _editProperties;
        private readonly SerializedProperty _fadeDurationProperty;

        static ScaleBlockDrawer()
        {
            space = EditorGUIUtility.standardVerticalSpacing;
            height = EditorGUIUtility.singleLineHeight;
            line = height + space;

            Texture image = Resources.Load<Texture>(PATH_ON_IMAGE);
            buttonOn = new(image, TOOLTIP_ON_BUTTON);

            image = Resources.Load<Texture>(PATH_OFF_IMAGE);
            buttonOff = new(image, TOOLTIP_OFF_BUTTON);

            //flatButtonStyle = new()
            //{
            //    name = "flatButton",
            //    alignment = TextAnchor.MiddleCenter,
            //    fixedHeight = height,
            //    fixedWidth = height
            //};
            //flatButtonStyle.normal.background = STYLES.BackgroundColor(new(56, 56, 56, 255));
            //flatButtonStyle.hover.background = STYLES.BackgroundColor(new(88, 88, 88, 255));
            //flatButtonStyle.focused.background = STYLES.BackgroundColor(new(118, 118, 118, 255));
            //flatButtonStyle.active.background = STYLES.BackgroundColor(new(38, 38, 38, 255));

            
        }

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
            _editProperties = new[]
            {
                colorBlock.FindPropertyRelative("_normalEdit"),
                colorBlock.FindPropertyRelative("_highlightedEdit"),
                colorBlock.FindPropertyRelative("_pressedEdit"),
                colorBlock.FindPropertyRelative("_selectedEdit"),
                colorBlock.FindPropertyRelative("_disabledEdit"),
            };

            _fadeDurationProperty = colorBlock.FindPropertyRelative("fadeDuration");
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
                        drawRect.y += line;
                        VectorField(drawRect, _colorProperties[i], _editProperties[i]);
                    }
                    indentLevel--;

                    if(editFade)
                    { 
                        drawRect.y += line; drawLine += 1f;
                        FadeDurationField(drawRect, _fadeDurationProperty);
                    }
                }
            }
            EndProperty();

            EditorGUILayout.EndVertical();

            if (_scaleBlockProperty.isExpanded) EditorGUILayout.Space(drawLine * line);
            else EditorGUILayout.Space(line);
        }

        private static void VectorField(Rect position, SerializedProperty vector, SerializedProperty editMode)
        {
            bool mode = editMode.boolValue;
            Vector3 oldScale = vector.vector3Value;

            if (GUI.Button(GetButtonRect(position), mode ? buttonOff : buttonOn, STYLES.flatButton))
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
            sizeButton.width = height;
            sizeButton.x = EditorGUIUtility.labelWidth - height * 0.5f;
            return sizeButton;
        }
    }
}
