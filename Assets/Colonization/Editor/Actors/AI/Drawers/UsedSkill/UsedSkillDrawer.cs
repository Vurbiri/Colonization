using UnityEditor;
using Vurbiri;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	internal static class SkillDrawer
	{
        public const string F_SKILL = "_skill";

        private static readonly string[][][] s_selfSkillNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_selfSkillValues = new int[ActorTypeId.Count][][];

        private static readonly string[][][] s_healNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_healValues = new int[ActorTypeId.Count][][];

        static SkillDrawer()
        {
            Update<WarriorsSettingsScriptable, WarriorId, WarriorSettings>(EUtility.FindAnyScriptable<WarriorsSettingsScriptable>(), ActorTypeId.Warrior);
            Update<DemonsSettingsScriptable, DemonId, DemonSettings>(EUtility.FindAnyScriptable<DemonsSettingsScriptable>(), ActorTypeId.Demon);
        }

        public static int Self(int type, int id, string label, SerializedProperty property) => Draw(label, property, s_selfSkillNames[type][id], s_selfSkillValues[type][id]);
        public static int Heal(int type, int id, string label, SerializedProperty property) => Draw(label, property, s_healNames[type][id], s_healValues[type][id]);

        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable, int type)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            scriptable.GetSelfSkills_Ed(ref s_selfSkillNames[type], ref s_selfSkillValues[type]);
            scriptable.GetHeals_Ed(ref s_healNames[type], ref s_healValues[type]);
        }

        private static int Draw(string label, SerializedProperty property, string[] names, int[] values)
        {
            int value = property.intValue;
            if(!values.Contains(value)) 
                value = - 1;
            return property.intValue = IntPopup(label, value, names, values);
        }
    }
}
