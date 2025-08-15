#if UNITY_EDITOR

using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class AnimationClipSettingsScriptable : ScriptableObject
    {
        private const string MENU = "Assets/Extract AnimationClip", WARRIOR = "Warrior", DEMON = "Demon";
        private static readonly string[] s_exclude = { "ZA_", "MA_" };
        private static readonly string[] s_folders = { "Melee", "Shield", "Wizard" };
        private static readonly System.Type s_type = typeof(AnimationClipSettingsScriptable);

        public AnimationClip clip;
        public float totalTime;
        public float[] hitTimes = new float[1];
        public float range = -1;
        public float distance = -1;

        [MenuItem(MENU, true)]
        private static bool ValidateSelection()
        {
            string animationName;
            foreach (var obj in Selection.objects)
            {
                if (obj is not AnimationClip animation)
                    return false;

                animationName = animation.name;
                foreach (var str in s_exclude)
                    if (animationName.StartsWith(str))
                        return false;

                if (EUtility.FindAnyScriptable<AnimationClipSettingsScriptable>(animationName.Replace("A_", "ACS_")) != null)
                    return false;
            }

            return true;
        }

        [MenuItem(MENU, false, 11)]
        private static void Creator()
        {
            string settingsName;
            AnimationClipSettingsScriptable settings;
            bool isCreate = false;

            foreach (var obj in Selection.objects)
            {
                if (obj is AnimationClip animation)
                {
                    settingsName = animation.name.Replace("A_", "ACS_");
                    settings = CreateScriptable(settingsName);
                    if (settings != null)
                    {
                        isCreate = true;
                        settings.clip = animation;
                        settings.name = settingsName;
                    }
                }
            }

            if (isCreate)
            {
                //AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

            #region Local CreateScriptable(..), TrySetPath(..)
            //======================================================
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static AnimationClipSettingsScriptable CreateScriptable(string defaultName)
            {
                if (!TrySetPath(defaultName, WARRIOR, s_folders, out string path))
                    TrySetPath(defaultName, DEMON, DemonId.Names_Ed, out path);

                AnimationClipSettingsScriptable asset = null;
                path = EditorUtility.SaveFilePanelInProject("Create AnimationClipSettings", defaultName, "asset", "", path);
                if (!string.IsNullOrEmpty(path))
                {
                    asset = (AnimationClipSettingsScriptable)CreateInstance(s_type);
                    AssetDatabase.CreateAsset(asset, path);
                    Debug.Log($"<b>[AnimationClip]</b> Creating <b>{defaultName}</b>\n<i>{path}</i>\n");
                }
                return asset;
            }
            //==============================================================
            static bool TrySetPath(string name, string mainFolder, string[] subFolders, out string path)
            {
                path = "Assets/Colonization/Settings/Editor/Animations";

                if (name.Contains(mainFolder))
                {
                    path = path.Concat("/", mainFolder);
                    foreach (var str in subFolders)
                    {
                        if (name.Contains(str))
                        {
                            path = path.Concat("/", str);
                            return true;
                        }
                    }
                    return true;
                }
                return false;
            }
            #endregion
        }

        private void OnValidate()
        {
            if (clip == null)
            {
                clip = EUtility.FindAnyAsset<AnimationClip>(name.Replace("ACS_", "A_"));
            }
        }
    }
}
#endif
