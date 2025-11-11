using UnityEditor;
using Vurbiri;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUILayout;

namespace VurbiriEditor.Colonization
{
	internal static class SkillDrawer
	{
        public const string F_SKILL = "_skill";

        private static readonly string[][][] s_skillNames = new string[ActorTypeId.Count][][];

        private static readonly string[][][] s_skillNamesNone  = new string[ActorTypeId.Count][][];
        private static readonly int[][][]    s_skillValuesNone = new int[ActorTypeId.Count][][];

        static SkillDrawer()
        {
            s_skillNames[ActorTypeId.Warrior] = EUtility.FindAnyScriptable<WarriorsSettingsScriptable>().SetSkills_Ed();
            s_skillNames[ActorTypeId.Demon]   = EUtility.FindAnyScriptable<DemonsSettingsScriptable>().SetSkills_Ed();

            SetNone(ref s_skillNamesNone[ActorTypeId.Warrior], ref s_skillValuesNone[ActorTypeId.Warrior], s_skillNames[ActorTypeId.Warrior]);
            SetNone(ref s_skillNamesNone[ActorTypeId.Demon]  , ref s_skillValuesNone[ActorTypeId.Demon]  , s_skillNames[ActorTypeId.Demon]);
        }

        public static int Draw(int type, int id, string label, int value) => Popup(label, value, s_skillNames[type][id]);
        public static int Draw(int type, int id, SerializedProperty property) => property.intValue = Popup(property.intValue, s_skillNames[type][id]);
        public static int Draw(int type, int id, string label, SerializedProperty property) => property.intValue = Popup(label, property.intValue, s_skillNames[type][id]);

        public static int DrawNone(int type, int id, string label, int value) => IntPopup(label, value, s_skillNamesNone[type][id], s_skillValuesNone[type][id]);
        public static int DrawNone(int type, int id, SerializedProperty property)
        {
            return property.intValue = IntPopup(property.intValue, s_skillNamesNone[type][id], s_skillValuesNone[type][id]);
        }
        public static int DrawNone(int type, int id, string label, SerializedProperty property)
        {
            return property.intValue = IntPopup(label, property.intValue, s_skillNamesNone[type][id], s_skillValuesNone[type][id]);
        }


        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            int type = typeof(TId) == typeof(WarriorId) ? ActorTypeId.Warrior : ActorTypeId.Demon;

            SetNone(ref s_skillNamesNone[type], ref s_skillValuesNone[type], s_skillNames[type] = scriptable.SetSkills_Ed());
        }

        private static void SetNone(ref string[][] names, ref int[][] values, string[][] namesSource)
        {
            int count = namesSource.Length;
            names  = new string[count][];
            values = new int[count][];

            for (int i = 0; i < count; i++)
                SetNone(ref names[i], ref values[i], namesSource[i]);

            // ============= Local ============
            static void SetNone(ref string[] names, ref int[] values, string[] namesSource)
            {
                int count = namesSource.Length;
                names  = new string[count + 1];
                values = new int[count + 1];

                names[0]  = "--------------";
                values[0] = -1;

                for (int i = 0, j = 1; i < count; i++, j++)
                {
                    names[j]  = namesSource[i];
                    values[j] = i;
                }
            }
        }
    }
}
