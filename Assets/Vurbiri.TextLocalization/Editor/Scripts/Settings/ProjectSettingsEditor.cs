//Assets\Vurbiri.TextLocalization\Editor\Scripts\Settings\ProjectSettingsEditor.cs
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VurbiriEditor;

namespace Vurbiri.TextLocalization.Editor
{
    using static CONST;

    [CustomEditor(typeof(ProjectSettingsScriptable), true)]
    internal class ProjectSettingsEditor : AEditorGetVE<ProjectSettingsEditor>
    {
        [SerializeField] private VisualTreeAsset _treeAsset;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = _treeAsset.CloneTree();

            var visualSettings = root.Q<VisualElement>("Object");

            var settings = root.Q<ObjectField>("Settings");
            settings.RegisterValueChangedCallback(OnSettings);

            Button buttonNew = root.Q<Button>("ButtonNew");
            buttonNew.clicked += OnButtonNew;

            return root;

            #region Local: OnSettings(...), OnButtonNew()
            //=================================
            void OnSettings(ChangeEvent<Object> changeEvent)
            {
                visualSettings.Clear();

                if (changeEvent.newValue is SettingsScriptable st)
                {
                    visualSettings.Add(SettingsEditor.CreateCachedEditorAndBind(st));
                    visualSettings.visible = true;
                }
                else
                {
                    visualSettings.visible = false;
                }
            }
            //=================================
            void OnButtonNew()
            {
                string path = EditorUtility.SaveFilePanelInProject("New settings", SETTINGS_NAME, ASSET_EXP, "", SETTINGS_PATH);
                if (string.IsNullOrEmpty(path))
                    return;

                SettingsScriptable settingsNew = CreateInstance<SettingsScriptable>();

                AssetDatabase.CreateAsset(settingsNew, path);
                ProjectSettingsScriptable.GetOrCreateSelf().CurrentSettings = settingsNew;
                //AssetDatabase.SaveAssets();
            }
            #endregion
        }
    }
}
