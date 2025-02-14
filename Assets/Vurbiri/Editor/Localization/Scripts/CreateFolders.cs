//Assets\Vurbiri\Editor\Localization\Scripts\CreateFolders.cs
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.Localization.Editor
{
    using static CONST_L;

    internal class CreateFolders
	{
        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            FileInfo languagesJson = new(FILE_PATH);

            if (!languagesJson.Exists)
            {
                Debug.Log("[Creating Assets/Localization/Resources/Languages.json]");
                languagesJson.Directory.Create();
                languagesJson.Create().Close();

                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
    }
}
