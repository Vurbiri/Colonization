using UnityEditor;

namespace VurbiriEditor.Colonization
{
	public class ChanceUsedSkillDrawer
	{
        private readonly SerializedProperty _skillProperty;
        private readonly SerializedProperty _chanceProperty;
        private readonly string _name;

        public ChanceUsedSkillDrawer(SerializedProperty parentProperty) : this(parentProperty, parentProperty.displayName) { }
        public ChanceUsedSkillDrawer(SerializedProperty parentProperty, string name)
        {
            _skillProperty  = parentProperty.FindPropertyRelative(SkillDrawer.F_SKILL);
            _chanceProperty = parentProperty.FindPropertyRelative("_chance").FindPropertyRelative("_value");

            _name = name;
        }

        public void Draw(int type, int id)
        {
            _skillProperty.intValue = SkillDrawer.Draw(type, id, _name, _skillProperty.intValue);

            if (_skillProperty.intValue >= 0)
                _chanceProperty.intValue = EditorGUILayout.IntSlider(" └─ Chance", _chanceProperty.intValue, 0, 100);
            else
                _chanceProperty.intValue = 0;
        }
    }
}
