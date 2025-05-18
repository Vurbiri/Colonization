//Assets\Colonization\Editor\Currencies\Windows\PricesWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{
    using static CONST_EDITOR;

    public class PricesWindow : EditorWindow
    {
        #region Consts
        private const string NAME = "Prices", MENU = MENU_PATH + NAME;
        #endregion

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
                Debug.Log("Нет PricesScriptable");
                return;
            }

            rootVisualElement.Add(PricesEditor.CreateCachedEditorAndBind(_prices));
        }
    }
}
