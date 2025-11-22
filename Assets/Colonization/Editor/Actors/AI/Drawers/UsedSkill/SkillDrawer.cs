using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Collections;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{

    internal static class SkillDrawer
	{
        private static readonly string[][][] s_defenseNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_defenseValues = new int[ActorTypeId.Count][][];

        private static readonly string[][] s_healNames = new string[ActorTypeId.Count][];
        private static readonly int[][] s_healValues = new int[ActorTypeId.Count][];

        private static readonly ReadOnlyIdArray<SkillType_Ed, Skills> s_skills = new(() => new());

        static SkillDrawer()
        {
            Update<WarriorsSettingsScriptable, WarriorId, WarriorSettings>(EUtility.FindAnyScriptable<WarriorsSettingsScriptable>(), ActorTypeId.Warrior);
            Update<DemonsSettingsScriptable, DemonId, DemonSettings>(EUtility.FindAnyScriptable<DemonsSettingsScriptable>(), ActorTypeId.Demon);
        }

        public static bool IsDefense(int type, int id)
        {
            var values = s_defenseValues[type][id];
            return values.Length > 1 || (values.Length == 1 && values[0] != -1);
        }
        public static int Defense(int type, int id, SerializedProperty property)
        {
            var values = s_defenseValues[type][id];
            int value = property.intValue;
            if (!values.Contains(value))
                value = -1;
            return property.intValue = IntPopup(property.displayName, value, s_defenseNames[type][id], values);
        }

        public static (string name, int value) GetHeals_Ed(int type, int id) => (s_healNames[type][id], s_healValues[type][id]);

        public static int GetCount(Id<SkillType_Ed> skill, int type, int id) => s_skills[skill].values[type][id].Length;
        public static GUIContent[] GetLabels(Id<SkillType_Ed> skill, int type, int id) => s_skills[skill].labels[type][id];
        public static int[] GetValues(Id<SkillType_Ed> skill, int type, int id) => s_skills[skill].values[type][id];

        public static void OnValidate(SerializedProperty property, int[] values, string name)
        {
            int count = values.Length;
            bool isValid = property.arraySize == count;
            if (isValid)
            {
                List<int> list = new(values);
                for(int i = 0; isValid & i < count; ++i)
                    isValid = list.Remove(property.GetArrayElementAtIndex(i).FindPropertyRelative(name).intValue);
            }

            if(!isValid)
            {
                property.arraySize = 0;
                property.serializedObject.ApplyModifiedProperties();
                property.arraySize = count;
                for (int i = 0, j = count - 1; i < count; ++i, --j)
                    property.GetArrayElementAtIndex(i).FindPropertyRelative(name).intValue = values[j];
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable, int type)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            scriptable.GetDefenseSkills_Ed(ref s_defenseNames[type], ref s_defenseValues[type]);
            scriptable.GetHeals_Ed(ref s_healNames[type], ref s_healValues[type]);

            for (int i = 0; i < SkillType_Ed.Count; ++i)
            {
                var skills = s_skills[i];
                scriptable.GetSkills_Ed(i, ref skills.labels[type], ref skills.values[type]);
            }
        }

        // ************ Nested *******************
        private class Skills
        {
            public readonly GUIContent[][][] labels = new GUIContent[ActorTypeId.Count][][];
            public readonly int[][][] values = new int[ActorTypeId.Count][][];
        }
    }
}
