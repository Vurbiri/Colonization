using System;
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
    [CustomPropertyDrawer(typeof(ChanceUsedSkills<>))]
    public class ChanceUsedSkillsDrawer : PropertyDrawer
    {
        private readonly int INDEX_TYPE = 0;
        private readonly string NAME_ARRAY = "_values";

        private Type _idType;
        private int _actorType;
        private int _count;

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

                        if (TrySetState())
                        {
                            arrayProperty.arraySize = _count;

                            indentLevel++;

                            for (int i = 0; i < _count; i++)
                            {
                                position.y += _height;
                                position = UsedSkillDrawer.Chance.Draw(_actorType, i, position, arrayProperty.GetArrayElementAtIndex(i));
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
            return _height * (mainProperty.isExpanded ? _count * UsedSkillDrawer.Chance.Height + 1f : 1f);
        }

        private bool TrySetState()
        {
            Type idType = fieldInfo.FieldType;
            if (idType.IsArray) idType = idType.GetElementType();
            idType = idType.GetGenericArguments()[INDEX_TYPE];

            bool isInit = idType == _idType;

            if (!isInit && IdTypeCache.Contain(idType))
            {
                _idType = idType;
                _actorType = idType == typeof(WarriorId) ? ActorTypeId.Warrior : ActorTypeId.Demon;
                _count = IdTypeCache.GetCount(idType);

                isInit = true;
            }

            return isInit;
        }
    }
}