//Assets\Vurbiri.International\Editor\Scripts\CreateFiles.cs
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.International.Editor
{
    using static CONST;

    internal class CreateFiles
	{

        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            FileInfo languagesJson = new(Application.dataPath.Concat(FILE_LANG_PATH));
            if (!languagesJson.Exists)
            {
                Log(FILE_LANG_PATH);
                languagesJson.Directory.Create();
                languagesJson.Create().Close();
            }
        }


        private static void Log(string path)
        {
            Debug.Log($"<b><color=red>[Created {path}]</color></b>");
        }
    }
}
