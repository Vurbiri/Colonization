namespace Vurbiri.Colonization
{
	public static class IntToStr
	{
        public static string ToStr(this int number)
        {
            string output;
            if (number >= MIN_NUMBERS_STR & number <= MAX_NUMBERS_STR)
                output = s_numbersStr[number - MIN_NUMBERS_STR];
            else
                output = number.ToString();
            return output;
        }

        public const int MIN_NUMBERS_STR = -101, MAX_NUMBERS_STR = 200;
        private static readonly string[] s_numbersStr = new string[MAX_NUMBERS_STR - MIN_NUMBERS_STR + 1];

        static IntToStr()
        {
            for (int i = 0, n = MIN_NUMBERS_STR; i < s_numbersStr.Length; i++, n++)
                s_numbersStr[i] = n.ToString();
        }
    }
}
