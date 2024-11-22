//Assets\Vurbiri\Editor\Utility\PathToCSFiles.cs
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    public class PathToCSFiles : AssetModificationProcessor
    {
		#region Consts
		private const string NAME = "Path To CSFiles", MENU = MENU_PATH + NAME;
        private const string META_EXT = ".meta", CS_EXT = ".cs";
        private const string MASK = "*" + CS_EXT, COMMENT = @"//", ASSETS = "Assets", START = COMMENT + ASSETS;
        #endregion

        private static uint _count;

        [MenuItem(MENU)]
		private static void Command()
		{
            string path = EditorUtility.OpenFolderPanel(NAME, Application.dataPath, "");

            if (string.IsNullOrEmpty(path))
                return;

            _count = 0;
            Search(path);
            EditorUtility.DisplayDialog(NAME, $"Изменено {_count} файлов", "OK");
        }

        public static void OnWillCreateAsset(string assetName)
        {
            if (!assetName.EndsWith(META_EXT)) return;
            assetName = assetName.Replace(META_EXT, string.Empty);
            if (!assetName.EndsWith(CS_EXT)) return;

            CreateComment(assetName);
        }

        public static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            if (!sourcePath.EndsWith(CS_EXT) || !destinationPath.EndsWith(CS_EXT) || !CommentFromPath(destinationPath, out string comment))
                return AssetMoveResult.DidNotMove;

            string[] lines = File.ReadAllLines(sourcePath, Encoding.UTF8);
            if (lines == null || lines.Length == 0)
                return AssetMoveResult.DidNotMove;

            string firstLine = lines[0];
            if (firstLine.Length == 0 || !firstLine.StartsWith(START))
                return AssetMoveResult.DidNotMove;

            lines[0] = comment;

            File.Delete(sourcePath);
            File.Move(string.Concat(sourcePath, META_EXT), string.Concat(destinationPath, META_EXT));
            File.WriteAllLines(destinationPath, lines, Encoding.UTF8);

            return AssetMoveResult.DidMove;
        }

        private static void Search(string path)
        {
            foreach (string dir in Directory.GetDirectories(path))
                Search(dir);

            foreach (string file in Directory.GetFiles(path, MASK))
                CreateComment(file);
        }

        private static void CreateComment(string path)
        {
            if (!CommentFromPath(path, out string comment))
                return;

            string firstLine = File.ReadLines(path, Encoding.UTF8).First();

            if (firstLine == null || firstLine.Length == 0)
                return;

            if (firstLine.StartsWith(START))
                Replace(path, comment);
            else
                Add(path, comment);

            _count++;
        }

        private static void Replace(string path, string comment)
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);

            lines[0] = comment;

            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

        private static void Add(string path, string comment)
        {
            string file = File.ReadAllText(path, Encoding.UTF8);

            StringBuilder stringBuilder = new(comment.Length + file.Length);
            stringBuilder.AppendLine(comment);
            stringBuilder.Append(file);

            File.WriteAllText(path, stringBuilder.ToString(), Encoding.UTF8);
        }

        private static bool CommentFromPath(string path, out string comment)
		{
            comment = null;

            int index = path.IndexOf(ASSETS);
            if (index == -1) return false;

            comment = string.Concat(COMMENT, path[index..].Replace(@"/", @"\"));
            return true;
        }
	}
}
