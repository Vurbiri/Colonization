using System.Collections.Generic;

namespace Vurbiri.International.Editor
{
	internal static class LanguageFiles
	{
		public static string[] names;
        public static int[] values;
        public static int count;

		static LanguageFiles()
		{
            names = Storage.LoadObjectFromResourceJson<string[]>(CONST.FILE_FILES);
            CreateValues();
        }

        public static void Set(List<string> files)
		{
            names = files.ToArray();
            CreateValues();
        }

        private static void CreateValues()
        {
            count = names.Length;
            values = new int[count];
            for (int i = 0; i < count; i++)
                values[i] = i;
        }
    }
}
