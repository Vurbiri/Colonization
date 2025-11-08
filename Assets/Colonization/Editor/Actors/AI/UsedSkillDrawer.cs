using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using static UnityEditor.EditorGUI;

namespace VurbiriEditor.Colonization
{
	internal static class UsedSkillDrawer
	{
        private const string F_SKILL = "_skill";

        private static readonly float s_height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static readonly string[][] s_fieldNames = new string[ActorTypeId.Count][];

        private static readonly string[][][] s_skillNames = new string[ActorTypeId.Count][][];
        private static readonly int[][][] s_skillValues = new int[ActorTypeId.Count][][];

        static UsedSkillDrawer()
        {

            SetNames(WarriorId.Names_Ed, ref s_fieldNames[ActorTypeId.Warrior]);
            SetNames(DemonId.Names_Ed, ref s_fieldNames[ActorTypeId.Demon]);

            EUtility.FindAnyScriptable<WarriorsSettingsScriptable>().SetSkills_Ed(ref s_skillNames[ActorTypeId.Warrior], ref s_skillValues[ActorTypeId.Warrior]);
            EUtility.FindAnyScriptable<DemonsSettingsScriptable>().SetSkills_Ed(ref s_skillNames[ActorTypeId.Demon], ref s_skillValues[ActorTypeId.Demon]);

            // ==== Local ====
            static void SetNames(string[] input, ref string[] output)
            {
                int count = input.Length; output = new string[count];
                for(int i = 0; i < count; i++)
                    output[i] = input[i].Concat(" Skill");
            }
        }

        public static int Draw(int type, int id, Rect position, int value)
        {
            return IntPopup(position, s_fieldNames[type][id], value, s_skillNames[type][id], s_skillValues[type][id]);
        }

        public static void Update<TScriptable, TId, TValue>(TScriptable scriptable)
             where TScriptable : ActorSettingsScriptable<TId, TValue>
             where TId : ActorId<TId> where TValue : ActorSettings
        {
            int type = typeof(TId) == typeof(WarriorId) ? ActorTypeId.Warrior : ActorTypeId.Demon;
            scriptable.SetSkills_Ed(ref s_skillNames[type], ref s_skillValues[type]);
        }

        public static class Chance
        {
            private const string F_CHANCE = "_chance", F_VALUE = "_value";

            public static float Height => 2f;

            public static Rect Draw(int type, int id, Rect position, SerializedProperty valueProperty)
            {
                var skillProperty = valueProperty.FindPropertyRelative(F_SKILL);
                var chanceProperty = valueProperty.FindPropertyRelative(F_CHANCE).FindPropertyRelative(F_VALUE);

                skillProperty.intValue = UsedSkillDrawer.Draw(type, id, position, skillProperty.intValue);

                indentLevel++;
                {
                    bool notSkill = skillProperty.intValue < 0;

                    position.y += s_height;
                    BeginDisabledGroup(notSkill);
                    {
                        chanceProperty.intValue = IntSlider(position, "Chance", notSkill ? 0 : chanceProperty.intValue, 0, 100);
                    }
                    EndDisabledGroup();
                }
                indentLevel--;

                return position;
            }
        }
    }
}
