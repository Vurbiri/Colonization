//Assets\Colonization\Editor\UI\Utility\CurrenciesIconsWindow.cs
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Vurbiri.Colonization.UI;
using Color = System.Drawing.Color;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class CurrenciesIconsWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Currencies Icons", MENU = MENU_UI_PATH + NAME;
        private const string PROPERTY_ICONS = "_icons";
        private const string BUTTON_NAME = "Create Atlas";
        private const string DEFAULT_PATH = "Assets/Colonization/Graphics/";
        private const string DEFAULT_NAME = "IconCurrency_Atlas";
        private const string EXP = "png";
        #endregion

        [SerializeField] private CurrenciesIconsScriptable _currenciesIcons;

        private SerializedObject _serializedObject;
        private SerializedProperty _iconsProperty;

        public static readonly Vector2 wndMinSize = new(350f, 350f);

        [MenuItem(MENU, false, 59)]
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
        }

        private void OnGUI()
        {
            if (_serializedObject == null)
                return;

            _serializedObject.Update();
            BeginWindows();

            EditorGUILayout.Space(SPACE_WND);
            EditorGUILayout.PropertyField(_iconsProperty);
            EditorGUILayout.Space(SPACE_WND);
            if (!Application.isPlaying && GUILayout.Button(BUTTON_NAME))
                CreateAtlas();

            EndWindows();
            _serializedObject.ApplyModifiedProperties();
        }


        private void CreateAtlas()
        {
            int iconsCount = _currenciesIcons.Icons.Count, ids = iconsCount;

            List<CurrencyBitmap> bitmaps = new(iconsCount);
            foreach (var icon in _currenciesIcons.Icons)
                bitmaps.Add(new(icon, --ids));

            Task[] tasks = new Task[iconsCount];
            for (int i = 0; i < iconsCount; i++)
            {
                tasks[i] = new(bitmaps[i].ReColor);
                tasks[i].Start();
            }
            Task.WaitAll(tasks);

            CurrencyBitmap bitmap = bitmaps[0];
            uint width = (uint)bitmap.width, height = (uint)(bitmap.height * iconsCount);
            int size = bitmap.colors.Length;

            Color32[] atlas = new Color32[size * iconsCount];

            Parallel.ForEach(bitmaps, (b) => {
                b.atlas = atlas;
                Parallel.For(0, size, b.Copy);
            });

            byte[] bytes = ImageConversion.EncodeArrayToPNG(atlas, GraphicsFormat.R8G8B8A8_UNorm, width, height);

            string path = UnityEditor.EditorUtility.SaveFilePanel("", DEFAULT_PATH, DEFAULT_NAME, EXP);

            if (string.IsNullOrEmpty(path))
                return;

            File.WriteAllBytes(path, bytes);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            foreach(var bm in bitmaps)
                bm.Dispose();
        }

        #region Nested: CurrencyBitmap
        //*******************************************************
        private class CurrencyBitmap : System.IDisposable
        {
            private const int BYTE = byte.MaxValue, COMPONENT_COUNT = 4;

            private readonly Color _newColor;
            private readonly Bitmap _bitmap;
            private readonly int _id;

            public readonly int width, height;
            public int size;
            public readonly Color32[] colors;
            public Color32[] atlas;

            public CurrencyBitmap(CurrencyIcon icon, int id)
            {
                _id = id;
                _newColor = ToSystemColor(icon.Color);
                _bitmap = new(AssetDatabase.GetAssetPath(icon.Icon), false);
                width = _bitmap.Width;
                height = _bitmap.Height;
                size = width * height;
                colors = new Color32[size];
            }

            public void ReColor()
            {
                int index = 0;
                for (int y = height - 1; y >= 0; y--)
                    for (int x = 0; x < width; x++)
                        colors[index++] = Mult(_bitmap.GetPixel(x, y), _newColor);
            }

            public void Copy(int j)
            {
                lock (this)
                {
                    atlas[size * _id + j] = colors[j];
                }
            }

            private Color32 Mult(Color cA, Color cB)
            {
                return new(M(cA.R, cB.R), M(cA.G, cB.G), M(cA.B, cB.B), M(cA.A, cB.A));

                // Local
                static byte M(float a, float b) => (byte)Mathf.RoundToInt((a * b) / BYTE);
            }

            private Color ToSystemColor(UnityEngine.Color color)
            {
                int[] c = new int[COMPONENT_COUNT];

                for (int i = 0; i < COMPONENT_COUNT; i++)
                    c[i] = Mathf.RoundToInt(color[i] * BYTE);

                return Color.FromArgb(c[3], c[0], c[1], c[2]);
            }

            public void Dispose()
            {
                _bitmap.Dispose();
            }
        }

        #endregion
    }
}
