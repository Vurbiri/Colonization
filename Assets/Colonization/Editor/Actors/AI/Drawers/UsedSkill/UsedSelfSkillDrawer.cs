using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.Colonization
{
	public class UsedSelfSkillDrawer
	{
        private readonly SerializedProperty _skillProperty;
        private readonly SerializedProperty _chanceProperty;
        private readonly GUIContent _label = new(" └─ Chance");
        private readonly string _name;

        public UsedSelfSkillDrawer(SerializedProperty parentProperty) : this(parentProperty, parentProperty.displayName) { }
        public UsedSelfSkillDrawer(SerializedProperty parentProperty, string name)
        {
            _skillProperty  = parentProperty.FindPropertyRelative(SkillDrawer.F_SKILL);
            _chanceProperty = parentProperty.FindPropertyRelative("_chance").FindPropertyRelative("_value");

            _name = name;
        }

        public void Draw(int type, int id)
        {
            if (SkillDrawer.Self(type, id, _name, _skillProperty) >= 0)
                _chanceProperty.intValue = EditorGUILayout.IntSlider(_label, _chanceProperty.intValue, 0, 100);
            else
                _chanceProperty.intValue = 0;
        }
    }
}
