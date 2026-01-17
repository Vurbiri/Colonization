using UnityEditor;
using UnityEngine;

namespace VurbiriEditor
{
	public class ARValueDrawer : PropertyDrawer
	{
		private const float LABEL_SIZE = 32f, RIGHT_SPACE = 6f;
        private const float FULL_SIZE = LABEL_SIZE * 2.5f + RIGHT_SPACE;

		protected const float MIN = 0.01f;
        protected readonly static GUIContent s_minLabel = new("Min"), s_maxLabel = new("Max");

		protected static (Rect, Rect, Rect, Rect, Rect) CalkPosition(Rect position)
		{
            float valueWidth = (position.width - EditorGUIUtility.labelWidth - FULL_SIZE) * 0.5f;
            float offset = EditorGUI.indentLevel * 15f;
			Rect labelSize = position, minLabelSize = position, minSize = position, maxLabelSize = position, maxSize = position;

			labelSize.width = EditorGUIUtility.labelWidth - offset + 2f;
			minLabelSize.width = maxLabelSize.width = LABEL_SIZE + offset;
			minSize.width = maxSize.width = valueWidth + offset;

            minLabelSize.x += labelSize.width;
			minSize.x = minLabelSize.x + LABEL_SIZE;

			maxLabelSize.x = minSize.x + valueWidth + LABEL_SIZE * 0.5f;
			maxSize.x = maxLabelSize.x + LABEL_SIZE;

			return (labelSize, minLabelSize, minSize, maxLabelSize, maxSize);
		}
    }
}
