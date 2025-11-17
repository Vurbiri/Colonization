using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public class UsedDefenseDrawer
    {
        private readonly int _typeId, _id;
        private readonly SerializedProperty _parentProperty;
        private readonly SerializedProperty _buffProperty;
        private readonly SerializedProperty _buffChanceProperty;
        private readonly SerializedProperty _blockChanceProperty;
        private readonly GUIContent _buffChanceLabel = new(" └─ Chance"), _blockChanceLabel = new("Block Chance");
        private readonly bool _isDefense, _isBlock;

        public UsedDefenseDrawer(SerializedProperty parentProperty, int typeId, int id)
        {
            _typeId = typeId; _id = id;

            _isDefense = SkillDrawer.IsDefense(typeId, id);
            _isBlock = typeId == ActorTypeId.Warrior;

            if (_isDefense | _isBlock)
                _parentProperty = parentProperty;

            if (_isDefense)
            {
                _buffProperty       = parentProperty.FindPropertyRelative("_buff");
                _buffChanceProperty = parentProperty.FindPropertyRelative("_buffChance").FindPropertyRelative("_value");
            }
            else
            {
                parentProperty.FindPropertyRelative("_buff").intValue = -1;
                parentProperty.FindPropertyRelative("_buffChance").FindPropertyRelative("_value").intValue = 0;
            }

            parentProperty.FindPropertyRelative("_block").boolValue = _isBlock;
            if (_isBlock)
                _blockChanceProperty = parentProperty.FindPropertyRelative("_blockChance").FindPropertyRelative("_value");
            else
                 parentProperty.FindPropertyRelative("_blockChance").FindPropertyRelative("_value").intValue = 0;
        }

        public void Draw()
        {
            if (_isDefense | _isBlock)
            {
                if (_parentProperty.isExpanded = Foldout(_parentProperty.isExpanded, _parentProperty.displayName))
                {
                    BeginVertical(STYLES.borderLight);
                    {

                        if (_isDefense)
                        {
                            if (SkillDrawer.Defense(_typeId, _id, _buffProperty) >= 0)
                                _buffChanceProperty.intValue = IntSlider(_buffChanceLabel, _buffChanceProperty.intValue, 0, 100);
                            else
                                _buffChanceProperty.intValue = 0;
                        }

                        if (_isBlock)
                            _blockChanceProperty.intValue = IntSlider(_blockChanceLabel, _blockChanceProperty.intValue, 0, 100);
                    }
                    EndVertical();
                }
            }
        }
    }
}
