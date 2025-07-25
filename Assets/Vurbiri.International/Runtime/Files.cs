using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Vurbiri.Storage;

namespace Vurbiri.International
{
	internal static class Files
	{
        private static readonly string[] s_files;

        public static readonly int Count;

        static Files()
        {
            s_files = LoadObjectFromResourceJson<string[]>(CONST_L.FILE_FILES);
            Throw.IfLengthZero(s_files, "s_files");

            Count = s_files.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Load(string folder, int fileId, out Dictionary<string, string> load)
        {
            return TryLoadObjectFromResourceJson(string.Concat(folder, "/", s_files[fileId]), out load);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(int index) => s_files[index];
        
        public static int IndexOf(string name)
        {
            for (int i = 0; i < Count; i++)
                if (s_files[i] == name)
                    return i;
            return -1;
        }
    }
}
