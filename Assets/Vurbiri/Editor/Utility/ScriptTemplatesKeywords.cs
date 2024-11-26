//Assets\Vurbiri\Editor\Utility\ScriptTemplatesKeywords.cs
using System.IO;
using UnityEditor;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    public class ScriptTemplatesKeywords : AssetModificationProcessor
	{
		private const string WINDOW = "Window", EDITOR = "Editor", DRAWER = "Drawer";
        
        public static void OnWillCreateAsset(string assetName)
		{
			if (!assetName.EndsWith(META_EXT)) return;
			assetName = assetName.Replace(META_EXT, string.Empty);
			if (!assetName.EndsWith(CS_EXT)) return;

			int index = Application.dataPath.LastIndexOf(ASSETS);
			string path = Path.Combine(Application.dataPath[..index], assetName);
			string file = File.ReadAllText(path, utf8WithoutBom);
			string name = Path.GetFileNameWithoutExtension(path);

			bool isReplace = false;
			isReplace |= Replace(ref file, @"#PROJECTNAME#", PlayerSettings.productName);
			isReplace |= Replace(ref file, @"#COMPANYNAME#", PlayerSettings.companyName);
            isReplace |= Replace(ref file, @"#NAMENOTWINDOW#", name.Replace(WINDOW, string.Empty));
            isReplace |= Replace(ref file, @"#NAMENOTEDITOR#", name.Replace(EDITOR, string.Empty));
			isReplace |= Replace(ref file, @"#NAMENOTDRAWER#", name.Replace(DRAWER, string.Empty));

			if (isReplace)
				File.WriteAllText(path, file, utf8WithoutBom);

			#region Local: Replace(..)
			//=======================================================
			static bool Replace(ref string file, string keyword, string replace)
			{
				if (file.IndexOf(keyword) < 0) 
					return false;

				file = file.Replace(keyword, replace);
				return true;
			}
			//=======================================================
			#endregion
		}
	}
}
