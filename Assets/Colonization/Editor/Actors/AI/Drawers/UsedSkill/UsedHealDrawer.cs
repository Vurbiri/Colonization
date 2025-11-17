using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	public class UsedHealDrawer
	{
        private readonly string _name;
        private readonly bool _isDraw;

        private readonly SerializedProperty _parentProperty;
        private readonly SerializedProperty _maxHPProperty;
        private readonly SerializedProperty _useSelfHPProperty;

        private readonly GUIContent _useSelfHPLabel = new("Use Self HP");
        private readonly GUIContent _maxHPLabelSelf = new("Max Self HP"), _maxHPLabelTarget = new("Max Target HP");

        public UsedHealDrawer(SerializedProperty parentProperty, int typeId, int id)
        {
            (string name, int value) = SkillDrawer.GetHeals_Ed(typeId, id);

            _name = name;
            parentProperty.FindPropertyRelative(SkillDrawer.F_SKILL).intValue = value;

            if (_isDraw = value >= 0)
            {
                _parentProperty    = parentProperty;
                _maxHPProperty     = parentProperty.FindPropertyRelative("_maxHP");
                _useSelfHPProperty = parentProperty.FindPropertyRelative("_useSelfHP");
            }
            else
            {
                parentProperty.FindPropertyRelative("_maxHP").intValue = 0;
                parentProperty.FindPropertyRelative("_useSelfHP").boolValue = false;
            }
        }

        public void Draw()
        {
            if (_isDraw)
            {
                if (_parentProperty.isExpanded = Foldout(_parentProperty.isExpanded, _name))
                {
                    BeginVertical(STYLES.borderLight);
                    {
                        PropertyField(_useSelfHPProperty, _useSelfHPLabel);
                        _maxHPProperty.intValue = IntSlider(_useSelfHPProperty.boolValue ? _maxHPLabelSelf : _maxHPLabelTarget, _maxHPProperty.intValue, 0, 100);
                    }
                    EndVertical();
                }
                Space();
            }
        }
    }
}
