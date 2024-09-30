using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Vurbiri.Colonization.UI;


namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class CurrenciesIconsWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Currencies Icons", MENU = MENU_PATH + NAME;
        private const string PROPERTY_ICONS = "_icons", PROPERTY_BLOOD = "_blood";
        private const string BUTTON_NAME = "Create Atlas";
        private const string DEFAULT_PATH = "Assets/TextMesh Pro/Sprites/";
        private const string DEFAULT_NAME = "IconCurrency_Atlas";
        private const string EXP = "png";
        private const int ICONS_COUNT = 6;
        #endregion


        [SerializeField] private CurrenciesIconsScriptable _currenciesIcons;

        private SerializedObject _serializedObject;
        private SerializedProperty _iconsProperty, _bloodProperty;

        public static readonly Vector2 wndMinSize = new(450f, 450f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<CurrenciesIconsWindow>(true, NAME).minSize = wndMinSize;
        }

        private void OnEnable()
        {
            if (_currenciesIcons == null)
                return;
            
            _serializedObject = new(_currenciesIcons);
            _iconsProperty = _serializedObject.FindProperty(PROPERTY_ICONS);
            _bloodProperty = _serializedObject.FindProperty(PROPERTY_BLOOD);
        }

        private void OnGUI()
        {
            if (_serializedObject == null)
                return;

            _serializedObject.Update();
            BeginWindows();

            EditorGUILayout.Space(SPACE_WND);
            EditorGUILayout.PropertyField(_iconsProperty);
            EditorGUILayout.PropertyField(_bloodProperty);
            EditorGUILayout.Space(SPACE_WND);
            if (!Application.isPlaying && GUILayout.Button(BUTTON_NAME))
                CreateAtlas();

            EndWindows();
            _serializedObject.ApplyModifiedProperties();
        }

        private void CreateAtlas()
        {
            CurrencyIcon blood = _currenciesIcons.Blood;

            List<CurrencyIcon> icons = new(ICONS_COUNT);
            icons.AddRange(_currenciesIcons.Icons);
            icons.Add(blood);
            icons.Reverse();

            Texture2D texture = blood.Icon.texture;
            int width = texture.width, height = texture.height * ICONS_COUNT;
            GraphicsFormat graphicsFormat = texture.graphicsFormat;

            NativeArray<Color32> inputPixels;
            List<Color32> atlas = new(width * height);

            foreach (var icon in icons)
            {
                inputPixels = icon.Icon.texture.GetPixelData<Color32>(0);

                using ReColorJobs reColor = new(inputPixels, icon.Color);
                JobHandle handle = reColor.Schedule(inputPixels.Length, 8);
                handle.Complete();

                atlas.AddRange(reColor.outputPixels);
            }

            byte[] bytes = ImageConversion.EncodeArrayToPNG(atlas.ToArray(), graphicsFormat, (uint)width, (uint)height);

            string path = EditorUtility.SaveFilePanel("", DEFAULT_PATH, DEFAULT_NAME, EXP);

            if (string.IsNullOrEmpty(path))
                return;

            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        #region Nested: ReColorJobs
        //*******************************************************
        private struct ReColorJobs : IJobParallelFor, IDisposable
        {
            [ReadOnly] private NativeArray<Color32> _inputPixels;
            [ReadOnly] private Color32 _inputColor;

            [WriteOnly] public NativeArray<Color32> outputPixels;

            public ReColorJobs(NativeArray<Color32> colors, Color newColor)
            {
                _inputPixels = new(colors, Allocator.TempJob);
                _inputColor = newColor;
                outputPixels = new(colors.Length, Allocator.TempJob);
            }

            public void Dispose()
            {
                _inputPixels.Dispose();
                outputPixels.Dispose();
            }

            public void Execute(int index)
            {
                outputPixels[index] = Multiplication(_inputPixels[index], _inputColor);

                // Local
                static Color32 Multiplication(Color colorA, Color colorB) => colorA * colorB;
            }
        }
        #endregion
    }
}
