using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vurbiri.Colonization.Characteristics;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class SatanAbilitiesWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Satan Abilities", MENU = MENU_CH_PATH + NAME;
        #endregion

        [SerializeField] private VisualTreeAsset _satanAbilitiesVT;
        [SerializeField] private SatanAbilities _abilities;

        [MenuItem(MENU, false, 21)]
		private static void ShowWindow()
		{
			GetWindow<SatanAbilitiesWindow>(true, NAME);
		}

        public void CreateGUI()
        {
            SettingsFileEditor.Load(ref _abilities);

            VisualElement element = _satanAbilitiesVT.CloneTree();
            element.Bind(new(this));
            rootVisualElement.Add(element);
        }

        private void OnDisable()
        {
            SettingsFileEditor.Save(_abilities);
        }
    }
}
