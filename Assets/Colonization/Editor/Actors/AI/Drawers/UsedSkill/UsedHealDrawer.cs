using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	public class UsedHealDrawer
	{
        private readonly string _name;
        private readonly bool _isDraw;

        private readonly SerializedProperty _parentProperty;
        private readonly SerializedProperty _cureProperty;
        private readonly SerializedProperty _usesSelfHPProperty;

        private readonly GUIContent _useSelfHPLabel = new("Uses Self HP");

        public UsedHealDrawer(SerializedProperty parentProperty, int typeId, int id)
        {
            (string name, int value) = SkillDrawer.GetHeals_Ed(typeId, id);

            _name = name;
            parentProperty.FindPropertyRelative(UsedHeal.skillField).intValue = value;

            if (_isDraw = value >= 0)
            {
                _parentProperty     = parentProperty;
                _cureProperty       = parentProperty.FindPropertyRelative(UsedHeal.cureField);
                _usesSelfHPProperty = parentProperty.FindPropertyRelative(UsedHeal.usesSelfHPField);
            }
            else
            {
                parentProperty.FindPropertyRelative(UsedHeal.cureField).boolValue = false;
                parentProperty.FindPropertyRelative(UsedHeal.usesSelfHPField).boolValue = false;
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
                        PropertyField(_cureProperty);
                        PropertyField(_usesSelfHPProperty, _useSelfHPLabel);
                    }
                    EndVertical();
                }
                Space();
            }
        }
    }
}
