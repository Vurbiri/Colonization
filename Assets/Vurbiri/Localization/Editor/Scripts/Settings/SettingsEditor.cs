using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.Localization.Editors
{
    using static CONST;

    [CustomEditor(typeof(SettingsScriptable), true)]
    internal class SettingsEditor : AEditorGetVE<SettingsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        protected override VisualElement Create(SerializedObject serializedObject)
        {
            VisualElement root = new();
            _treeAsset.CloneTree(root);

            TextField folderPath = root.Q<TextField>("FolderPath");
            TextField folder = root.Q<TextField>("Folder");
            TextField filePath = root.Q<TextField>("FilePath");
            TextField languagesFile = root.Q<TextField>("LanguagesFile");

            folderPath.SetEnabled(false);
            folder.SetEnabled(false);
            filePath.SetEnabled(false);
            languagesFile.SetEnabled(false);

            Button languagesFileButton = root.Q<Button>("LanguagesFileButton");
            languagesFileButton.clicked += OnLanguagesFileButton;

            return root;

            #region Local: OnLanguagesFileButton()
            //=================================
            void OnLanguagesFileButton()
            {
                string result = EditorUtility.OpenFilePanel("", folderPath.value, "json");

                if (string.IsNullOrEmpty(result))
                    return;

                languagesFile.value = Path.GetFileNameWithoutExtension(result);
                folder.value = Directory.GetParent(result).Name;

                filePath.value = result.Split(ASSET_FOLDER).Last();
                folderPath.value = Path.GetDirectoryName(filePath.value).Replace('\\', '/');
            }
            #endregion
        }
    }
}
