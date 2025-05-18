//Assets\Vurbiri.EntryPoint\Editor\EntryPointExecutionOrder.cs
using System;
using UnityEditor;
using UnityEngine;

namespace Vurbiri.EntryPoint.Editor
{
    [InitializeOnLoad]
    internal class EntryPointExecutionOrder : AssetPostprocessor
    {
        #region Consts
        private const int SCENE_ORDER = -15, PROJECT_ORDER = 5;
        private const string MENU_NAME = "EntryPoints/", MENU = "Vurbiri/" + MENU_NAME;
        private const string MENU_NAME_SET = "Set Execution Order", MENU_COMMAND_SET = MENU + MENU_NAME_SET;
        private const string MENU_NAME_AUTO = "Auto", MENU_COMMAND_AUTO = MENU + MENU_NAME_AUTO;
        private const string CS_EXT = ".cs";
        private const string KEY_SAVE = "EPEO_AUTO";
        #endregion

        private static bool s_isAuto = true;

        private static readonly Type s_monoType = typeof(MonoBehaviour);
        private static readonly Type s_sceneType = typeof(ASceneEntryPoint), _projectType = typeof(AProjectEntryPoint);

        static EntryPointExecutionOrder()
        {
            if (EditorPrefs.HasKey(KEY_SAVE))
                s_isAuto = EditorPrefs.GetBool(KEY_SAVE);
        }

        [MenuItem(MENU_COMMAND_SET)]
        private static void CommandSet()
        {
            foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
                SetOrders(monoScript);

            Debug.Log($"<b><color=yellow>[EntryPointExecutionOrder] End set order.</color></b>");
        }

        [MenuItem(MENU_COMMAND_AUTO, false, 13)]
        private static void CommandAuto()
        {
            s_isAuto = !s_isAuto;

            EditorPrefs.SetBool(KEY_SAVE, s_isAuto);
            SetChecked();
            Log();
        }
        [MenuItem(MENU_COMMAND_AUTO, true, 13)]
        private static bool CommandAutoValidate()
        {
            SetChecked();
            return true;
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (!s_isAuto) return;

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
            
            SetOrders(monoScript);
        }

        private static void SetOrders(MonoScript monoScript)
        {
            Type currentType = monoScript.GetClass();
            if (currentType == null || currentType.IsAbstract) return;

            currentType = currentType.BaseType;
            if (currentType == null) return;

            SetOrder(monoScript, currentType, s_sceneType, SCENE_ORDER);
            SetOrder(monoScript, currentType, _projectType, PROJECT_ORDER);
        }

        private static void SetOrder(MonoScript monoScript, Type currentType, Type type, int order)
        {
            if (!currentType.Is(type, s_monoType)) return;

            if (MonoImporter.GetExecutionOrder(monoScript) != order)
            {
                MonoImporter.SetExecutionOrder(monoScript, order);
                EditorUtility.DisplayDialog("Entry Point Execution Order", $"{monoScript.name} = {order}", "OK");
            }
        }

        private static void SetChecked()
        {
            Menu.SetChecked(MENU_COMMAND_AUTO, s_isAuto);
            Menu.SetChecked(MENU, s_isAuto);
        }

        private static void Log()
        {
            string state = s_isAuto ? "Enable" : "Disable";
            string color = s_isAuto ? "green" : "red";
            Debug.Log($"<color={color}>[EntryPointExecutionOrder] <b>{state}<b></color>");
        }
    }
}
