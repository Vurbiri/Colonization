using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public static class CONST
    {
        public const int MOVE_SKILL_ID = 5, SPEC_SKILL_ID = 6;
        public const int BLESS_SKILL_ID = 0, WRATH_SKILL_ID = 1;

        public const int WALL_TYPE = ActorTypeId.Demon + 1, SPELL_TYPE = WALL_TYPE + 1;

        public const int WALL_DURATION = 1, WALL_SKIP = 9, WALL_ADD_SHIFT = 3, WALL_SHIFT = ActorAbilityId.SHIFT_ABILITY + WALL_ADD_SHIFT;
        public const int BLOCK_DURATION = 1, BLOCK_SKIP = 0;

        public const int MAX_CIRCLES = 4;

        public const int CHANCE_WATER = MAX_CIRCLES * MAX_CIRCLES;

        public const int DEFAULT_MAX_WARRIOR = 5;
        public const int DEFAULT_MAX_DEMONS = DEFAULT_MAX_WARRIOR << 1;
        public const int DEFAULT_MAX_EDIFICES = 6;

        public const int DICES_COUNT = 2;
        public const int DICE_MAX = 6;
        public static readonly ReadOnlyArray<int> DICE = new(new int[] { 1, 2, 3, 4, 5, 6 });

        public const float PI = Mathf.PI;
        public const float TAU = 2f * PI;

        public const float COS_00 = 1f;
        public const float COS_30 = 0.86602540378f;
        public const float COS_60 = 0.5f;
        public const float COS_90 = 0f;

        public const float SIN_00 = COS_90;
        public const float SIN_30 = COS_60;
        public const float SIN_60 = COS_30;
        public const float SIN_90 = COS_00;

        public static string ToStr(this int number)
        {
            string output;
            if (number >= MIN_NUMBERS_STR & number <= MAX_NUMBERS_STR)
                output = s_numbersStr[number - MIN_NUMBERS_STR];
            else
                output = number.ToString();
            return output;
        }

        public const int MIN_NUMBERS_STR = -101, MAX_NUMBERS_STR = 198;
        private static readonly string[] s_numbersStr = new string[MAX_NUMBERS_STR - MIN_NUMBERS_STR + 1];

        static CONST()
        {
            for (int i = 0, n = MIN_NUMBERS_STR; i < s_numbersStr.Length; i++, n++)
                s_numbersStr[i] = n.ToString();
        }
    }
}

