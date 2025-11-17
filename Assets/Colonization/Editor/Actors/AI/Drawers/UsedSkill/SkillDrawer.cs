using UnityEditor;
using Vurbiri;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	internal static class SkillDrawer
	{
        public const string F_SKILL = "_skill";

        private static readonly string[][][] s_defenseNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_defenseValues = new int[ActorTypeId.Count][][];

        private static readonly string[][][] s_selfSkillNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_selfSkillValues = new int[ActorTypeId.Count][][];

        private static readonly string[][] s_healNames = new string[ActorTypeId.Count][];
        private static readonly int[][] s_healValues = new int[ActorTypeId.Count][];

        static SkillDrawer()
        {
            Update<WarriorsSettingsScriptable, WarriorId, WarriorSettings>(EUtility.FindAnyScriptable<WarriorsSettingsScriptable>(), ActorTypeId.Warrior);
            Update<DemonsSettingsScriptable, DemonId, DemonSettings>(EUtility.FindAnyScriptable<DemonsSettingsScriptable>(), ActorTypeId.Demon);
        }

        public static bool IsDefense(int type, int id) => IsValues(s_defenseValues[type][id]);
        public static int Defense(int type, int id, SerializedProperty property) => Draw(property.displayName, property, s_defenseNames[type][id], s_defenseValues[type][id]);

        public static (string name, int value) GetHeals_Ed(int type, int id) => (s_healNames[type][id], s_healValues[type][id]);

        public static int SelfCount(int type, int id) => s_selfSkillValues[type][id].Length;
        public static string GetSelfName(int type, int id, int index) => s_selfSkillNames[type][id][index];
        public static int[] GetSelfValues(int type, int id) => s_selfSkillValues[type][id];
        public static int Self(int type, int id, string label, SerializedProperty property) => Draw(label, property, s_selfSkillNames[type][id], s_selfSkillValues[type][id]);

        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable, int type)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            scriptable.GetDefenseSkills_Ed(ref s_defenseNames[type], ref s_defenseValues[type]);
            scriptable.GetSelfSkills_Ed(ref s_selfSkillNames[type], ref s_selfSkillValues[type]);
            scriptable.GetHeals_Ed(ref s_healNames[type], ref s_healValues[type]);
        }

        private static bool IsValues(int[] values) => values.Length > 1 || (values.Length == 1 && values[0] != -1);

        private static int Draw(string label, SerializedProperty property, string[] names, int[] values)
        {
            int value = property.intValue;
            if(!values.Contains(value)) 
                value = - 1;
            return property.intValue = IntPopup(label, value, names, values);
        }
    }
}
