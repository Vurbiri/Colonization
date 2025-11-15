using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
    public class UsedDefenseDrawer
    {
        private readonly SerializedProperty _parentProperty;
        private readonly SerializedProperty _buffProperty;
        private readonly SerializedProperty _buffChanceProperty;
        private readonly SerializedProperty _blockProperty;
        private readonly SerializedProperty _blockChanceProperty;
        private readonly GUIContent _buffChanceLabel = new(" └─ Chance"), _blockChanceLabel = new("Block Chance");

        public UsedDefenseDrawer(SerializedProperty parentProperty)
        {
            _parentProperty      = parentProperty;
            _buffProperty        = parentProperty.FindPropertyRelative("_buff");
            _buffChanceProperty  = parentProperty.FindPropertyRelative("_buffChance").FindPropertyRelative("_value");
            _blockProperty       = parentProperty.FindPropertyRelative("_block");
            _blockChanceProperty = parentProperty.FindPropertyRelative("_blockChance").FindPropertyRelative("_value");
        }

        public void Draw(int type, int id)
        {

            if (_parentProperty.isExpanded = Foldout(_parentProperty.isExpanded, _parentProperty.displayName))
            {
                BeginVertical(STYLES.borderLight);
                {
                    
                    if (SkillDrawer.Self(type, id, _buffProperty) >= 0)
                        _buffChanceProperty.intValue = IntSlider(_buffChanceLabel, _buffChanceProperty.intValue, 0, 100);
                    else
                        _buffChanceProperty.intValue = 0;

                    if (_blockProperty.boolValue = type == ActorTypeId.Warrior)
                        _blockChanceProperty.intValue = IntSlider(_blockChanceLabel, _blockChanceProperty.intValue, 0, 100);
                    else
                        _blockChanceProperty.intValue = 0;
                    
                }
                EndVertical();
            }
        }
    }
}
