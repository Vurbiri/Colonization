using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
	internal static class SkillDrawer
	{
        private static readonly string[][][] _skillNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] _skillValues = new int[ActorTypeId.Count][][];

        static SkillDrawer()
        {
            EUtility.FindAnyScriptable<WarriorsSettingsScriptable>().SetSkills_Ed(ref _skillNames[ActorTypeId.Warrior], ref _skillValues[ActorTypeId.Warrior]);
            EUtility.FindAnyScriptable<DemonsSettingsScriptable>().SetSkills_Ed(ref _skillNames[ActorTypeId.Demon], ref _skillValues[ActorTypeId.Demon]);
        }

        public static int Draw(int type, int id, Rect position, string label, int value)
        {
            return EditorGUI.IntPopup(position, label, value, _skillNames[type][id], _skillValues[type][id]);
        }

        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            int type = typeof(TId) == typeof(WarriorId) ? ActorTypeId.Warrior : ActorTypeId.Demon;
            scriptable.SetSkills_Ed(ref _skillNames[type], ref _skillValues[type]);
        }
    }
}
