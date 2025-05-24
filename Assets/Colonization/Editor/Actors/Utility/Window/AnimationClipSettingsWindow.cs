using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri;
using Vurbiri.Colonization.Actors;

namespace VurbiriEditor.Colonization.Actors
{
    using static CONST_EDITOR;

    public class AnimationClipSettingsWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _treeAnimationClipSettingsWindow;

        private const string NAME = "Animation Clips Settings", MENU = MENU_PATH + NAME;
        private const string NAME_MELEE = "Melee", NAME_SHIELD = "Shield", NAME_WIZAED = "Wizard";

        private readonly List<Editor> _editors = new();

        [MenuItem(MENU, false, 51)]
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

            var containerMelee = root.Q<ScrollView>(NAME_MELEE);
            var containerShield = root.Q<ScrollView>(NAME_SHIELD);
            var containerWizard = root.Q<ScrollView>(NAME_WIZAED);

            AnimationClipSettingsScriptable clip;
            Editor editor;

            for (int i = settings.Count - 1; i >= 0; i--)
            {
                clip = settings[i];
                editor = null;

                NameSynchronization(clip);

                if (clip.name.Contains(NAME_MELEE))
                    containerMelee.Add(AnimationClipSettingsEditor.CreateEditorAndBind(clip, out editor));

                if (clip.name.Contains(NAME_SHIELD))
                    containerShield.Add(AnimationClipSettingsEditor.CreateEditorAndBind(clip, out editor));

                if (clip.name.Contains(NAME_WIZAED))
                    containerWizard.Add(AnimationClipSettingsEditor.CreateEditorAndBind(clip, out editor));

                if (editor)
                    _editors.Add(editor);
            }

            rootVisualElement.Add(root);
        }

        private void NameSynchronization(AnimationClipSettingsScriptable scriptable)
        {
            AnimationClip clip = scriptable.clip;

            if (clip == null)
                return;

            string newName = clip.name.Insert(1, "CS");

            if (scriptable.name != newName)
            {
                string path = AssetDatabase.GetAssetPath(scriptable);
                AssetDatabase.RenameAsset(path, newName);
                scriptable.name = newName;

                //Selection.activeObject = scriptable;
            }
        }

        private void OnDisable()
        {
            foreach (var editor in _editors)
                DestroyImmediate(editor);

            _editors.Clear();

            var warriorsSettings = EUtility.FindAnyScriptable<WarriorsSettingsScriptable>();
            if (warriorsSettings != null)
                ActorUtility.OverrideClips(warriorsSettings.Settings);
        }
    }
}
