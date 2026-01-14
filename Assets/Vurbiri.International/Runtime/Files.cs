using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Vurbiri.JsonResources;

namespace Vurbiri.International
{
	internal static class Files
	{
        private static readonly string[] s_files;

        public static readonly int Count;

        static Files()
        {
            s_files = Load<string[]>(CONST_L.FILES_FILE);
            Throw.IfLengthZero(s_files, "s_files");

            Count = s_files.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLoad(string folder, int fileId, out Dictionary<string, string> load)
        {
            return JsonResources.TryLoad(string.Concat(folder, "/", s_files[fileId]), out load);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(int index) => s_files[index];
        
        public static int IndexOf(string name)
        {
            int i = Count;
            while (i --> 0 && s_files[i] != name);
            return i;
        }
    }
}
