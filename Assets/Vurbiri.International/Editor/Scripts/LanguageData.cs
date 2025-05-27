using System;
using System.Collections.Generic;

namespace Vurbiri.International.Editor
{
	internal static class LanguageData
	{
		public static string[] fileNames;
        public static int[] fileValues;
        public static int fileCount;

        public static string[][] keys;

        public static string folder;
        static LanguageData()
		{
            folder = Storage.LoadObjectFromResourceJson<LanguageType[]>(CONST_L.FILE_LANG)[0].Folder;
            fileNames = Storage.LoadObjectFromResourceJson<string[]>(CONST.FILE_FILES);
            CreateValues();
        }

        public static void SetFiles(List<string> files)
		{
            fileNames = files.ToArray();
            CreateValues();
        }

        public static void CreateKeys(string name) => CreateKeys(Array.IndexOf(fileNames, name));

        private static void CreateValues()
        {
            fileCount = fileNames.Length;
            fileValues = new int[fileCount];
            keys = new string[fileCount][];
            for (int i = 0; i < fileCount; i++)
            {
                fileValues[i] = i;
                CreateKeys(i);
            }
        }

        private static void CreateKeys(int idFile)
        {
            var dict = Storage.LoadObjectFromResourceJson<Dictionary<string, string>>(string.Concat(folder, "/", fileNames[idFile]));

            keys[idFile] = new string[dict.Count + 1];

            int index = 0;
            keys[idFile][index++] = "── None ──";
            foreach( var key in dict.Keys )
                keys[idFile][index++] = key;
        }
    }
}
