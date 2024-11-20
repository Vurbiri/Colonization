using System.IO;
using UnityEditor;
using UnityEngine;
using Vurbiri;

namespace VurbiriEditor
{
    public class ScriptTemplatesKeywords : AssetModificationProcessor
    {
        private const string META_EXT = ".meta", CS_EXT = ".cs";
        private const string ASSETS = "Assets", EDITOR = "Editor", DRAWER = "Drawer";
        //private const string NAMESPACE = "namespace ";

        public static void OnWillCreateAsset(string assetName)
        {
            if (!assetName.EndsWith(META_EXT)) return;
            assetName = assetName.Replace(META_EXT, string.Empty);
            if (!assetName.EndsWith(CS_EXT)) return;

            int index = Application.dataPath.LastIndexOf(ASSETS);
            string path = Path.Combine(Application.dataPath[..index], assetName);
            string file = File.ReadAllText(path);
            string name = Path.GetFileNameWithoutExtension(path);

            file = file.Replace(@"#PATH#", assetName);
            file = file.Replace(@"#DATE#", System.DateTime.Now.ToString());
            file = file.Replace(@"#PROJECTNAME#", PlayerSettings.productName);
            file = file.Replace(@"#COMPANYNAME#", PlayerSettings.companyName);
            file = file.Replace(@"#NAMENOTEDITOR#", name.Replace(EDITOR, string.Empty));
            file = file.Replace(@"#NAMENOTDRAWER#", name.Replace(DRAWER, string.Empty));
            file = file.Replace(@"#NAMESPACEEDITOR#", PlayerSettings.companyName.Concat(EDITOR));
            //file = file.Replace(@"#ROOTNAMESPACEEDITORBEGIN#", NAMESPACE.Concat(PlayerSettings.companyName, EDITOR,"\n\r{"));
            //file = file.Replace(@"#ROOTNAMESPACEEDITOREND#", "}\n\r");

            File.WriteAllText(path, file);

            AssetDatabase.Refresh();
        }
    }
}
