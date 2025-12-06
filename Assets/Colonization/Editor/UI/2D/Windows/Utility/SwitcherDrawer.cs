using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization.UI
{
	[CustomPropertyDrawer(typeof(Switcher))]
	public class SwitcherDrawer : PropertyDrawer
	{
		private readonly string F_SWITCHER = "_canvasSwitcher", F_GROUP = "_canvasGroup", F_SPEED = "_speed";
		private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
            mainProperty.isExpanded = true;
            position.height = EditorGUIUtility.singleLineHeight;

			var switcherProperty = mainProperty.FindPropertyRelative(F_SWITCHER);

            label = BeginProperty(position, label, mainProperty);
			{
				if (switcherProperty.isExpanded = Foldout(position, switcherProperty.isExpanded, label))
				{
					indentLevel++;
					position.y += _height;
					PropertyField(position, switcherProperty.FindPropertyRelative(F_GROUP));
                    position.y += _height;
                    PropertyField(position, switcherProperty.FindPropertyRelative(F_SPEED));
                    indentLevel--;
                }
			}
			EndProperty();
		}
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return _height * (mainProperty.FindPropertyRelative(F_SWITCHER).isExpanded ? 3f : 1f);
		}
	}
}