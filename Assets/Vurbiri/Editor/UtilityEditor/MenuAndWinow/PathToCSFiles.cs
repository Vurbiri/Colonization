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
    public class PathToCSFiles //: AssetModificationProcessor
    {
		#region Consts
		private const string MENU_NAME = "Path To CSFiles/", MENU = MENU_PATH + MENU_NAME;
        private const string MENU_NAME_ADD = "Add path", MENU_COMMAND_ADD = MENU + MENU_NAME_ADD;
        private const string MENU_NAME_REMOVE = "Remove path", MENU_COMMAND_REMOVE = MENU + MENU_NAME_REMOVE;
        private const string MENU_NAME_AUTO = "Auto", MENU_COMMAND_AUTO = MENU + MENU_NAME_AUTO;
        private const string MASK = "*" + CS_EXT, COMMENT = @"//", START = COMMENT + ASSETS;
        #endregion

        private static uint s_count;
        private static bool s_isAuto = false;
        private static readonly string s_key_save = Application.productName + "_PTCS_AUTO";

        static PathToCSFiles()
        {
            if (EditorPrefs.HasKey(s_key_save))
                s_isAuto = EditorPrefs.GetBool(s_key_save);
        }

        [MenuItem(MENU_COMMAND_ADD)]
		private static void CommandAdd()
		{
            string path = EditorUtility.OpenFolderPanel(MENU_NAME_ADD, Application.dataPath, "");

            if (string.IsNullOrEmpty(path))
                return;

            s_count = 0;
            SearchAndCreate(path);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(MENU_NAME_ADD, $"Изменено {s_count} файлов", "OK");
        }
        [MenuItem(MENU_COMMAND_REMOVE)]
        private static void CommandRemove()
        {
            string path = EditorUtility.OpenFolderPanel(MENU_NAME_REMOVE, Application.dataPath, "");

            if (string.IsNullOrEmpty(path))
                return;

            s_count = 0;
            SearchAndRemove(path);
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(MENU_NAME_REMOVE, $"Очищено {s_count} файлов", "OK");
        }

        [MenuItem(MENU_COMMAND_AUTO, false, 82)]
        private static void CommandAuto()
        {
            s_isAuto = !s_isAuto;

            EditorPrefs.SetBool(s_key_save, s_isAuto);
            SetChecked();
            Log();
        }
        [MenuItem(MENU_COMMAND_AUTO, true, 82)]
        private static bool CommandAutoValidate()
        {
            SetChecked();
            return true;
        }

        public static void OnWillCreateAsset(string assetName)
        {
            if (!s_isAuto) return;
            
            if (!assetName.EndsWith(META_EXT)) return;
            assetName = assetName.Replace(META_EXT, string.Empty);
            if (!assetName.EndsWith(CS_EXT)) return;

            CreateComment(assetName);
        }

        public static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            AssetMoveResult notMove = AssetMoveResult.DidNotMove;

            if (!s_isAuto) return notMove;

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

        private static void SearchAndCreate(string path)
        {
            foreach (string file in Directory.GetFiles(path, MASK, SearchOption.AllDirectories))
                CreateComment(file);
        }

        private static void SearchAndRemove(string path)
        {
            foreach (string file in Directory.GetFiles(path, MASK, SearchOption.AllDirectories))
                RemoveComment(file);
        }

        private static void CreateComment(string path)
        {
            if (!CommentFromPath(path, out string comment))
                return;

            string firstLine = File.ReadLines(path, utf8WithoutBom).First();

            if (string.IsNullOrEmpty(firstLine))
                return;

            if (firstLine.StartsWith(START))
                Replace(path, comment);
            else
                Add(path, comment);

            s_count++;
        }

        private static void RemoveComment(string path)
        {
            string[] lines = File.ReadAllLines(path, utf8WithoutBom);
            int length = lines.Length;

            if (length == 0 || !lines[0].StartsWith(START))
                return;

            if (--length == 0)
            {
                File.WriteAllText(path, string.Empty, utf8WithoutBom);
            }
            else
            {
                string[] newLines = new string[length];
                for (int i = 0; i < length; i++)
                    newLines[i] = lines[i + 1];

                File.WriteAllLines(path, newLines, utf8WithoutBom);
            }

            s_count++;
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
            Menu.SetChecked(MENU_COMMAND_AUTO, s_isAuto);
            //Menu.SetChecked(MENU, isAuto);
        }

        private static void Log()
        {
            string state = s_isAuto ? "Enable" : "Disable";
            string color = s_isAuto ? "green" : "red";
            Debug.Log($"<color={color}>[PathToCSFiles] <b>{state}</b></color>");
        }
	}
}
