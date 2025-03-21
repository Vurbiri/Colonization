//Assets\Vurbiri\Editor\EntryPoint\EntryPointExecutionOrder.cs
using System;
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.EntryPoint;
using static VurbiriEditor.CONST_EDITOR;

namespace VurbiriEditor.EntryPoint
{
    [InitializeOnLoad]
    internal class EntryPointExecutionOrder : AssetPostprocessor
    {
        #region Consts
        private const int SCENE_ORDER = -15, PROJECT_ORDER = 5;
        private const string MENU_NAME = "EntryPoints/", MENU = MENU_PATH + MENU_NAME;
        private const string MENU_NAME_SET = "Set Execution Order", MENU_COMMAND_SET = MENU + MENU_NAME_SET;
        private const string MENU_NAME_AUTO = "Auto", MENU_COMMAND_AUTO = MENU + MENU_NAME_AUTO;
        private const string KEY_SAVE = "EPEO_AUTO";
        #endregion

        private static bool isAuto = true;

        private static readonly Type _monoType = typeof(MonoBehaviour);
        private static readonly Type _sceneType = typeof(ASceneEntryPoint), _projectType = typeof(AProjectEntryPoint);

        static EntryPointExecutionOrder()
        {
            if (EditorPrefs.HasKey(KEY_SAVE))
                isAuto = EditorPrefs.GetBool(KEY_SAVE);
            Menu.SetChecked(MENU_COMMAND_AUTO, isAuto);
        }

        [MenuItem(MENU_COMMAND_SET)]
        private static void CommandSet()
        {
            foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
            {
                SetOrder(monoScript, _sceneType, SCENE_ORDER);
                SetOrder(monoScript, _projectType, PROJECT_ORDER);
            }
        }
        [MenuItem(MENU_COMMAND_AUTO, false, 13)]
        private static void CommandAuto()
        {
            isAuto = !isAuto;

            EditorPrefs.SetBool(KEY_SAVE, isAuto);
            Menu.SetChecked(MENU_COMMAND_AUTO, isAuto);
            Log();
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (!isAuto) return;

            for (int i = importedAssets.Length - 1; i >= 0; i--)
                SetExecutionOrder(importedAssets[i]);
        }

        private static void SetExecutionOrder(string assetName)
        {
            if (!assetName.EndsWith(CS_EXT)) return;

            var monoImporter = AssetImporter.GetAtPath(assetName) as MonoImporter;
            if (monoImporter == null) return;

            var monoScript = monoImporter.GetScript();
            if (monoScript == null) return;
            
            SetOrder(monoScript, _sceneType, SCENE_ORDER);
            SetOrder(monoScript, _projectType, PROJECT_ORDER);
        }

        private static void SetOrder(MonoScript monoScript, Type type, int order)
        {
            Type currentType = monoScript.GetClass()?.BaseType;
            if (currentType == null || !currentType.Is(type, _monoType)) return;

            if (MonoImporter.GetExecutionOrder(monoScript) != order)
            {
                MonoImporter.SetExecutionOrder(monoScript, order);
                EditorUtility.DisplayDialog("Entry Point Execution Order", $"{monoScript.name} = {order}", "OK");
            }
        }

        private static void Log()
        {
            string state = isAuto ? "Enable" : "Disable";
            Debug.Log($"[EntryPointExecutionOrder] {state}");
        }
    }
}
