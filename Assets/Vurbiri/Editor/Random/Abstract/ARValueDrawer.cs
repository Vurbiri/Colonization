using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
    public class ARValueDrawer : PropertyDrawer
    {
        private const float VALUE_SIZE = 85f, SIZE_SPACE = 6f;

        protected static (Rect, Rect, Rect) CalkPosition(Rect position)
        {
            float offset = EditorGUI.indentLevel * 15f;
            Rect labelSize = position, minSize = position, maxSize = position;

            labelSize.width = EditorGUIUtility.labelWidth - offset + 2f;
            minSize.width = maxSize.width = VALUE_SIZE + offset;
            
            minSize.x += labelSize.width;
            maxSize.x = minSize.x + VALUE_SIZE + SIZE_SPACE;

            return (labelSize, minSize, maxSize);
        }
    }
}
