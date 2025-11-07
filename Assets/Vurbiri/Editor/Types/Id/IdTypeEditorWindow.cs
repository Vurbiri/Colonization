using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor
{
    public class IdTypeEditorWindow : EditorWindow
    {
        private const string NAME = "IdType Editor", MENU = MENU_PATH + NAME;

        private readonly List<string> _paths = new(), _names = new();
        //private string[] _namesIdType;

        [MenuItem(MENU, false, 80)]
        private static void ShowWindow()
        {
            GetWindow<IdTypeEditorWindow>(true, NAME);
        }

        private void OnEnable()
        {
            string mask = "*" + CS_EXT;
            string classPattern = @"\bclass\s+{0}\s*:";

            List<string> names = new();
            foreach (var type in IdTypeCache.Types)
            {
                if (!type.IsNested)
                    names.Add(type.Name);
            }

            //_namesIdType = names.ToArray();

            foreach (string file in Directory.GetFiles(Application.dataPath, mask, SearchOption.AllDirectories))
            {
                string fileContent = File.ReadAllText(file);
                for (int i = names.Count - 1; i >= 0; i--)
                {
                    if (Regex.IsMatch(fileContent, string.Format(classPattern, names[i])))
                    {
                        _names.Add(names[i]);
                        _paths.Add(FileUtil.GetPhysicalPath(file));
                        names.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void OnGUI()
        {
            BeginWindows();
            Draw();
            EndWindows();

            #region Local: Draw
            //=================================
            void Draw()
            {
                for (int i = _paths.Count - 1; i >= 0; i--)
                    EditorGUILayout.LabelField(_names[i], _paths[i]);

            }
            #endregion
        }

        private void OnDisable()
        {

        }
    }
}