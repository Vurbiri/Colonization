//Assets\Vurbiri\Editor\Random\Abstract\ARValueDrawer.cs
using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public class ARValueDrawer : PropertyDrawer
    {
        private const float OFFSET_SIZE_LABEL = 20f, SIZE_VALUE = 50f, BIGSIZE_VALUE = 85f, SIZE_SPACE = 5f;

        protected (Rect, Rect, Rect) CalkPosition(Rect position)
        {
            Rect sizeLabel = position, sizeMin = position, sizeMax = position;
            sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;
            sizeMin.x = sizeLabel.width;
            sizeMax.x = sizeLabel.width + BIGSIZE_VALUE + SIZE_SPACE;
            sizeMin.width = sizeMax.width = BIGSIZE_VALUE;

            return (sizeLabel, sizeMin, sizeMax);
        }

        protected (Rect, Rect, Rect, Rect) CalkPositionSlider(Rect position)
        {
            Rect sizeLabel = position, sizeMin = position, sizeMax = position, sizeSlider = position;
            sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;
            sizeMin.x = sizeLabel.width;
            sizeSlider.x = sizeLabel.width + SIZE_VALUE + SIZE_SPACE;
            sizeMax.x = EditorGUIUtility.currentViewWidth - SIZE_VALUE;

            sizeMin.width = sizeMax.width = SIZE_VALUE;
            sizeSlider.width = EditorGUIUtility.currentViewWidth - SIZE_VALUE * 2f - sizeLabel.width - SIZE_SPACE * 2f;

            return (sizeLabel, sizeMin, sizeSlider, sizeMax);
        }

        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
    }
}
