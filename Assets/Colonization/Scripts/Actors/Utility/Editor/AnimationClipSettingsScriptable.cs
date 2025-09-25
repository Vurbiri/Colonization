#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class AnimationClipSettingsScriptable : ScriptableObject
    {
        private const string MENU = "Assets/Extract AnimationClip", WARRIOR = "Warrior", DEMON = "Demon";
        private static readonly string[] s_exclude = { "ZA_", "MA_", "RA_" };
        private static readonly string[] s_folders = { "Melee", "Shield", "Wizard" };
        private static readonly System.Type s_type = typeof(AnimationClipSettingsScriptable);

        public AnimationClip clip;
        public float totalTime;
        public float totalTimeRatio = 100f;
        public float[] hitTimes = new float[1];

        [MenuItem(MENU, true)]
        private static bool ValidateSelection()
        {
            foreach (var obj in Selection.objects)
                if (ObjectValidate(obj, out _))
                    return true;

            return false;
        }

        [MenuItem(MENU, false, 11)]
        private static void Creator()
        {
            AnimationClipSettingsScriptable settings;
            bool isCreate = false;

            foreach (var obj in Selection.objects)
            {
                if (obj is AnimationClip animation)
                {
                    if (ObjectValidate(obj, out string settingsName))
                    {
                        settings = CreateScriptable(settingsName);
                        if (settings != null)
                        {
                            isCreate = true;
                            settings.clip = animation;
                            settings.name = settingsName;
                        }
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

        private static bool ObjectValidate(Object obj, out string settingsName)
        {
            settingsName = null;
            if (obj is not AnimationClip clip)
                return false;

            foreach (var str in s_exclude)
                if (clip.name.StartsWith(str))
                    return false;

            settingsName = clip.name.Replace("A_", "ACS_");
            if (EUtility.FindAnyScriptable<AnimationClipSettingsScriptable>(settingsName) != null)
                return false;

            return true;
        }

        private void OnValidate()
        {
            if (clip == null)
                clip = EUtility.FindAnyAsset<AnimationClip>(name.Replace("ACS_", "A_"));

            if (clip != null && totalTime < 0.1f)
                totalTime = clip.length;

            if (totalTimeRatio > 100f)
                totalTimeRatio = 100f;
        }
    }
}
#endif
