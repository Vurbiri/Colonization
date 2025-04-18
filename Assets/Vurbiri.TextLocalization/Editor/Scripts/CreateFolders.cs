//Assets\Vurbiri.TextLocalization\Editor\Scripts\CreateFolders.cs
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.TextLocalization.Editor
{
    using static CONST_L;

    internal class CreateFolders
	{
        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            FileInfo languagesJson = new(Application.dataPath.Concat(FILE_PATH));

            if (!languagesJson.Exists)
            {
                Debug.Log($"<b><color=red>[Creating /Assets{FILE_PATH}]</color></b>");
                languagesJson.Directory.Create();
                languagesJson.Create().Close();

                //AssetDatabase.Refresh();
                //AssetDatabase.SaveAssets();
            }
        }
    }
}
