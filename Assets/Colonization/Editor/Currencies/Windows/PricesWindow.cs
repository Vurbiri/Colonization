using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PricesWindow : EditorWindow
    {
        private const string NAME = "Prices", MENU = MENU_PATH + NAME;

        [SerializeField] private Prices _prices;

        [MenuItem(MENU, false, 30)]
        private static void ShowWindow()
        {
            GetWindow<PricesWindow>(false, NAME).minSize = new(325f, 400f);
        }

        public void CreateGUI()
        {
            if (_prices == null)
            {
                _prices = EUtility.FindAnyScriptable<Prices>();
                if (_prices == null)
                    _prices = EUtility.CreateScriptable<Prices>("Prices", "Assets/Colonization/Settings");
                else
                    Debug.LogWarning($"Set Prices");
            }

            rootVisualElement.Add(PricesEditor.CreateCachedEditorAndBind(_prices));
        }
    }
}
