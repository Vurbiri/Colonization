using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(SpellId))]
	public class SpellIdDrawer : PropertyDrawer
	{
		private readonly string F_TYPE = "type", F_ID = "id";
		private readonly string[] _typeNames = { "Economic", "Military" };

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
		
		public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
		{
			position.height = EditorGUIUtility.singleLineHeight;

			var typeProperty = mainProperty.FindPropertyRelative(F_TYPE);
            var idProperty = mainProperty.FindPropertyRelative(F_ID);

			int type = typeProperty.intValue;

			label.text = "Spell";

            label = BeginProperty(position, label, mainProperty);
			{
				LabelField(position, label, EditorStyles.boldLabel);
				position.y += _height;
				indentLevel++;

				typeProperty.intValue = type = Popup(position, "Type", type, _typeNames, EditorStyles.popup);
				position.y += _height;

				string[] idNames = type == 0 ? EconomicSpellId.Names_Ed : MilitarySpellId.Names_Ed;
				idProperty.intValue = Popup(position, "Id", idProperty.intValue, idNames, EditorStyles.popup);

				indentLevel--;
			}
			EndProperty();
        }
		
		public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
		{
			return _height * 3f;
		}
	}
}