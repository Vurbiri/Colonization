using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class AnimationClipSettingsWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _treeAnimationClipSettingsWindow;

        private const string NAME = "Animation Clips Settings", MENU = MENU_AC_PATH + NAME;
        
        private readonly string[] WARRIOR_NAMES = { "Melee", "Shield", "Wizard" };
        private readonly List<Editor> _editors = new();

        [MenuItem(MENU)]
        public static void ShowWindow()
        {
            GetWindow<AnimationClipSettingsWindow>(true, NAME).minSize = new(370f, 500f);
        }

        public void CreateGUI()
        {
            List<AnimationClipSettingsScriptable> settings = EUtility.FindScriptables<AnimationClipSettingsScriptable>();

            if (settings == null || settings.Count == 0 || _treeAnimationClipSettingsWindow == null)
                return;

            var root = _treeAnimationClipSettingsWindow.CloneTree();

            Dictionary<string, ScrollView> containers = new(8);

            for(int i = 0; i < 3; ++i)
                containers["ACS_Warrior_".Concat(WARRIOR_NAMES[i])] = root.Q<ScrollView>(WARRIOR_NAMES[i]);

            for (int i = 0; i < DemonId.Count; ++i)
                containers["ACS_Demon_".Concat(DemonId.Names_Ed[i])] = root.Q<ScrollView>(DemonId.Names_Ed[i]);

            AnimationClipSettingsScriptable clip;
            Editor editor; string name;

            for (int i = settings.Count - 1; i >= 0; --i)
            {
                editor = null;

                if (string.IsNullOrEmpty(name = NameSynchronization(clip = settings[i])))
                {
                    Debug.LogWarning($"[AnimationClips] Клип <b>{clip.name}</b> не назначен.");
                    continue;
                }

                foreach(var container in containers)
                {
                    if (name.StartsWith(container.Key))
                    {
                        container.Value.Add(AnimationClipSettingsEditor.CreateEditorAndBind(clip, out editor));
                        break;
                    }
                }

                if (editor) 
                    _editors.Add(editor);
                else
                    Debug.LogWarning($"[AnimationClips] Клип <b>{name}</b> не идентифицирован.");
            }

            root.Q<Button>("Apply_1").clicked += Apply;
            root.Q<Button>("Apply_2").clicked += Apply;

            rootVisualElement.Add(root);
        }

        private string NameSynchronization(AnimationClipSettingsScriptable scriptable)
        {
            AnimationClip clip = scriptable.clip;

            if (clip == null)
                return null;

            string newName = clip.name.Insert(1, "CS");

            if (scriptable.name != newName)
            {
                string path = AssetDatabase.GetAssetPath(scriptable);
                AssetDatabase.RenameAsset(path, newName);
                scriptable.name = newName;
            }

            return scriptable.name;
        }

        private void Apply()
        {
            var warriorsSettings = EUtility.FindAnyScriptable<WarriorsSettingsScriptable>();
            if (warriorsSettings != null)
                warriorsSettings.UpdateAnimation_Ed();
        }

        private void OnDisable()
        {
            foreach (var editor in _editors)
                DestroyImmediate(editor);

            _editors.Clear();
        }
    }
}
