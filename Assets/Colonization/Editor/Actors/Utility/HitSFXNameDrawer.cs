using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(HitSFXName))]
	public class HitSFXNameDrawer : PropertyDrawer
	{
        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            if (SFXFactoriesStorage.names_ed == null)
                return;

            position.height = EditorGUIUtility.singleLineHeight;
            var nameProperty = mainProperty.FindPropertyRelative("_value");
            var names = SFXFactoriesStorage.names_ed;
            var name = nameProperty.stringValue;
            var index = names.Length;

            while (index --> 1 && names[index] != name);

            label = BeginProperty(position, label, mainProperty);
            {
                index = Popup(position, label.text, index, names);
                nameProperty.stringValue = names[index];
            }
            EndProperty();
        }
    }
}