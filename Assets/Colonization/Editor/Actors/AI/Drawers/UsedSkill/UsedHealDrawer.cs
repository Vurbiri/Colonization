using UnityEditor;
using UnityEngine;

namespace VurbiriEditor.Colonization
{
	public class UsedHealDrawer
	{
        private readonly SerializedProperty _skillProperty;
        private readonly SerializedProperty _maxHPProperty;
        private readonly SerializedProperty _useSelfHPProperty;
        private readonly string _name;
        private readonly GUIContent _useSelfHPLabel = new(" ├─ Use Self HP");
        private readonly GUIContent _maxHPLabelSelf = new(" └─ Max Self HP"), _maxHPLabelTarget = new(" └─ Max Target HP");

        public UsedHealDrawer(SerializedProperty parentProperty) : this(parentProperty, parentProperty.displayName) { }
        public UsedHealDrawer(SerializedProperty parentProperty, string name)
        {
            _skillProperty     = parentProperty.FindPropertyRelative(SkillDrawer.F_SKILL);
            _maxHPProperty     = parentProperty.FindPropertyRelative("_maxHP");
            _useSelfHPProperty = parentProperty.FindPropertyRelative("_useSelfHP");
            _name = name;
        }

        public void Draw(int type, int id)
        {
            if (SkillDrawer.Heal(type, id, _name, _skillProperty) >= 0)
            {
                EditorGUILayout.PropertyField(_useSelfHPProperty, _useSelfHPLabel);
                _maxHPProperty.intValue = EditorGUILayout.IntSlider(_useSelfHPProperty.boolValue ? _maxHPLabelSelf : _maxHPLabelTarget, _maxHPProperty.intValue, 0, 100);
            }
            else
            {
                _maxHPProperty.intValue = 0;
                _useSelfHPProperty.boolValue = false;
            }
        }
    }
}
