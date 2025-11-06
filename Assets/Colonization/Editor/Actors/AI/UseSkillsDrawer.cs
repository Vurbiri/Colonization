using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	[CustomPropertyDrawer(typeof(UsedSkills<>))]
	public class UseSkillsDrawer : PropertyDrawer
	{
        private readonly int INDEX_TYPE = 0;
        private readonly string NAME_ARRAY = "_values";

        private Type _idType;
        private bool _isWarrior;
        private int _count;
        private string[] _fieldNames;

        private string[][] _skillNames;
        private int[][] _skillValues;

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
                        SerializedProperty propertyValues = mainProperty.FindPropertyRelative(NAME_ARRAY);
                        SerializedProperty propertyValue;

                        if (TrySetState())
                        {
                            propertyValues.arraySize = _count;

                            indentLevel++;
                            for (int i = 0; i < _count; i++)
                            {
                                propertyValue = propertyValues.GetArrayElementAtIndex(i);
                                position.y += _height;
                                propertyValue.intValue = IntPopup(position, _fieldNames[i], propertyValue.intValue, _skillNames[i], _skillValues[i]);
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
			float rate = mainProperty.isExpanded ? _count + 1f : 1f;

			return _height * rate;
		}

        private bool TrySetState()
        {
            Type idType = fieldInfo.FieldType;
            if (idType.IsArray) idType = idType.GetElementType();
            idType = idType.GetGenericArguments()[INDEX_TYPE];

            bool isInit = idType == _idType & _fieldNames != null;

            if (!isInit && IdCacheEd.Contain(idType))
            {
                _idType = idType;
                _isWarrior = idType == typeof(WarriorId);
                _count = IdCacheEd.GetCount(idType);
                _fieldNames = IdCacheEd.GetNames(idType);

                if (_isWarrior)
                    EUtility.FindAnyScriptable<WarriorsSettingsScriptable>().SetSkills_Ed(ref _skillNames, ref _skillValues, "Блок");
                else
                    EUtility.FindAnyScriptable<WarriorsSettingsScriptable>().SetSkills_Ed(ref _skillNames, ref _skillValues);

                isInit = true;
            }

            return isInit;
        }

    }
}