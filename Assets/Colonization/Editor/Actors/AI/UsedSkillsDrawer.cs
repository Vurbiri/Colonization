using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(UsedSkills<>))]
    public class UsedSkillsDrawer : PropertyDrawer
    {
        private readonly int INDEX_TYPE = 0;
        private readonly string NAME_ARRAY = "_values", NAME_SKILL = "skill", NAME_CHANCE = "chance", NAME_VALUE = "_value";

        private Type _idType;
        private int _actorType;
        private int _count;
        private string[] _fieldNames;

        private readonly float _height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty mainProperty, GUIContent label)
        {
            if (TrySetState())
            {

                position.height = EditorGUIUtility.singleLineHeight;

                label = BeginProperty(position, label, mainProperty);
                {
                    if (mainProperty.isExpanded = Foldout(position, mainProperty.isExpanded, label))
                    {
                        SerializedProperty arrayProperty = mainProperty.FindPropertyRelative(NAME_ARRAY);
                        SerializedProperty valueProperty, skillProperty, chanceProperty;

                        if (TrySetState())
                        {
                            arrayProperty.arraySize = _count;

                            indentLevel++;
                            for (int i = 0; i < _count; i++)
                            {
                                valueProperty = arrayProperty.GetArrayElementAtIndex(i);
                                skillProperty = valueProperty.FindPropertyRelative(NAME_SKILL);
                                chanceProperty = valueProperty.FindPropertyRelative(NAME_CHANCE).FindPropertyRelative(NAME_VALUE);

                                position.y += _height;
                                skillProperty.intValue = SkillDrawer.Draw(_actorType, i, position, _fieldNames[i], skillProperty.intValue);

                                indentLevel++;
                                position.y += _height;
                                bool notSkill = skillProperty.intValue < 0;
                                BeginDisabledGroup(notSkill);
                                {
                                    chanceProperty.intValue = IntSlider(position, "Chance", notSkill ? 0 : chanceProperty.intValue, 0, 100);
                                }
                                EndDisabledGroup();
                                indentLevel--;
                            }
                            indentLevel--;
                        }
                    }
                }
                EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty mainProperty, GUIContent label)
        {
            float rate = mainProperty.isExpanded ? _count * 2f + 1f : 1f;

            return _height * rate;
        }

        private bool TrySetState()
        {
            Type idType = fieldInfo.FieldType;
            if (idType.IsArray) idType = idType.GetElementType();
            idType = idType.GetGenericArguments()[INDEX_TYPE];

            bool isInit = idType == _idType & _fieldNames != null;

            if (!isInit && IdTypeCache.Contain(idType))
            {
                _idType = idType;
                _actorType = idType == typeof(WarriorId) ? ActorTypeId.Warrior : ActorTypeId.Demon;
                _count = IdTypeCache.GetCount(idType);

                var names = IdTypeCache.GetNames(idType);
                _fieldNames = new string[_count];
                for (int i = 0; i < _count; i++)
                    _fieldNames[i] = string.Concat(names[i], " Skill");

                isInit = true;
            }

            return isInit;
        }
    }
}