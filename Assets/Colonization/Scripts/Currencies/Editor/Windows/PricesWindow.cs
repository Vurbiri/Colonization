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

        [SerializeField] private PricesScriptable _prices;

        public static readonly Vector2 wndMinSize = new(325f, 400f);

        [MenuItem(MENU)]
        private static void ShowWindow()
        {
            GetWindow<PricesWindow>(false, NAME).minSize = wndMinSize;
        }

        public void CreateGUI()
        {
            if (_prices == null)
            {
                Debug.Log("��� PricesScriptable");
                return;
            }

            rootVisualElement.Add(PricesScriptableEditor.GetVisualElement(new(_prices)));
        }
    }
}
