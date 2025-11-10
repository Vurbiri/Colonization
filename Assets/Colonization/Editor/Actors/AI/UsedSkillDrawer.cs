using UnityEditor;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
	internal static class SkillDrawer
	{
        public const string F_SKILL = "_skill";

        private static readonly string[][][] s_skillNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_skillValues = new int[ActorTypeId.Count][][];

        static SkillDrawer()
        {
            EUtility.FindAnyScriptable<WarriorsSettingsScriptable>().SetSkills_Ed(ref s_skillNames[ActorTypeId.Warrior], ref s_skillValues[ActorTypeId.Warrior]);
            EUtility.FindAnyScriptable<DemonsSettingsScriptable>()  .SetSkills_Ed(ref s_skillNames[ActorTypeId.Demon],   ref s_skillValues[ActorTypeId.Demon]);
        }

        public static int Draw(int type, int id, string label, int value)
        {
            return EditorGUILayout.IntPopup(label, value, s_skillNames[type][id], s_skillValues[type][id]);
        }

        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            int type = typeof(TId) == typeof(WarriorId) ? ActorTypeId.Warrior : ActorTypeId.Demon;
            scriptable.SetSkills_Ed(ref s_skillNames[type], ref s_skillValues[type]);
        }
    }
}
