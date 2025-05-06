//Assets\Vurbiri\Editor\UtilityEditor\PathToCSFiles.cs
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    [InitializeOnLoad]
    public class PathToCSFiles : AssetModificationProcessor
    {
		#region Consts
		private const string MENU_NAME = "Path To CSFiles/", MENU = MENU_PATH + MENU_NAME;
        private const string MENU_NAME_ADD = "Add path", MENU_COMMAND_ADD = MENU + MENU_NAME_ADD;
        private const string MENU_NAME_AUTO = "Auto", MENU_COMMAND_AUTO = MENU + MENU_NAME_AUTO;
        private const string MASK = "*" + CS_EXT, COMMENT = @"//", START = COMMENT + ASSETS;
        private const string KEY_SAVE = "PTCS_AUTO";
        #endregion

        private static uint count;
        private static bool isAuto = false;

        static PathToCSFiles()
        {
            if (EditorPrefs.HasKey(KEY_SAVE))
                isAuto = EditorPrefs.GetBool(KEY_SAVE);
        }

        [MenuItem(MENU_COMMAND_ADD)]
		private static void CommandAdd()
		{
            string path = EditorUtility.OpenFolderPanel(MENU_NAME_ADD, Application.dataPath, "");

            if (string.IsNullOrEmpty(path))
                return;

            count = 0;
            Search(path);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(MENU_NAME_ADD, $"Изменено {count} файлов", "OK");
        }

        [MenuItem(MENU_COMMAND_AUTO, false, 12)]
        private static void CommandAuto()
        {
            isAuto = !isAuto;

            EditorPrefs.SetBool(KEY_SAVE, isAuto);
            SetChecked();
            Log();
        }
        [MenuItem(MENU_COMMAND_AUTO, true, 12)]
        private static bool CommandAutoValidate()
        {
            SetChecked();
            return true;
        }

        public static void OnWillCreateAsset(string assetName)
        {
            if (!isAuto) return;
            
            if (!assetName.EndsWith(META_EXT)) return;
            assetName = assetName.Replace(META_EXT, string.Empty);
            if (!assetName.EndsWith(CS_EXT)) return;

            CreateComment(assetName);
        }

        public static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            AssetMoveResult notMove = AssetMoveResult.DidNotMove;

            if (!isAuto) return notMove;

            if (!sourcePath.EndsWith(CS_EXT) || !destinationPath.EndsWith(CS_EXT) || !CommentFromPath(destinationPath, out string comment))
                return notMove;

            string[] lines = File.ReadAllLines(sourcePath, utf8WithoutBom);
            if (lines == null || lines.Length == 0)
                return notMove;

            string firstLine = lines[0];
            if (firstLine.Length == 0 || !firstLine.StartsWith(START))
                return notMove;

            lines[0] = comment;

            File.Delete(sourcePath);
            File.Move(sourcePath.Concat(META_EXT), destinationPath.Concat(META_EXT));
            File.WriteAllLines(destinationPath, lines, utf8WithoutBom);

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

            string firstLine = File.ReadLines(path, utf8WithoutBom).First();

            if (firstLine == null || firstLine.Length == 0)
                return;

            if (firstLine.StartsWith(START))
                Replace(path, comment);
            else
                Add(path, comment);

            count++;
        }

        private static void Replace(string path, string comment)
        {
            string[] lines = File.ReadAllLines(path, utf8WithoutBom);

            lines[0] = comment;

            File.WriteAllLines(path, lines, utf8WithoutBom);
        }

        private static void Add(string path, string comment)
        {
            string file = File.ReadAllText(path, utf8WithoutBom);

            StringBuilder stringBuilder = new(comment.Length + file.Length);
            stringBuilder.AppendLine(comment);
            stringBuilder.Append(file);

            File.WriteAllText(path, stringBuilder.ToString(), utf8WithoutBom);
        }

        private static bool CommentFromPath(string path, out string comment)
		{
            comment = null;

            int index = path.IndexOf(ASSETS);
            if (index < 0) return false;

            comment = string.Concat(COMMENT, path[index..].Replace(@"/", @"\"));
            return true;
        }

        private static void SetChecked()
        {
            Menu.SetChecked(MENU_COMMAND_AUTO, isAuto);
            //Menu.SetChecked(MENU, isAuto);
        }

        private static void Log()
        {
            string state = isAuto ? "Enable" : "Disable";
            string color = isAuto ? "green" : "red";
            Debug.Log($"<color={color}>[PathToCSFiles] <b>{state}</b></color>");
        }
	}
}
