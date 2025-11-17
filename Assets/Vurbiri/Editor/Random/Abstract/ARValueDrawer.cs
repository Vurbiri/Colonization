using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public class ARValueDrawer : PropertyDrawer
    {
        private const float OFFSET_SIZE_LABEL = 20f, BIGSIZE_VALUE = 85f, SIZE_SPACE = 5f;

        protected static (Rect, Rect, Rect) CalkPosition(Rect position)
        {
            Rect sizeLabel = position, sizeMin = position, sizeMax = position;
            sizeLabel.width = EditorGUIUtility.labelWidth + OFFSET_SIZE_LABEL;
            sizeMin.x = sizeLabel.width;
            sizeMax.x = sizeLabel.width + BIGSIZE_VALUE + SIZE_SPACE;
            sizeMin.width = sizeMax.width = BIGSIZE_VALUE;

            return (sizeLabel, sizeMin, sizeMax);
        }
    }
}
