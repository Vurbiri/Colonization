//Assets\Colonization\Editor\Characteristic\Abilities\Windows\SatanAbilitiesWindow.cs
using UnityEditor;
using UnityEngine;
using Vurbiri;
using Vurbiri.Colonization;
using VurbiriEditor.Colonization.Characteristics;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    public class SatanAbilitiesWindow : EditorWindow
	{
		#region Consts
		private const string NAME = "Satan Abilities", MENU = MENU_CH_PATH + NAME;
		#endregion
		
		[SerializeField] private SatanAbilitiesScriptable _scriptable;
		
		[MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<SatanAbilitiesWindow>(true, NAME);
		}

        public void CreateGUI()
        {
            if (_scriptable == null)
                _scriptable = EUtility.FindAnyScriptable<SatanAbilitiesScriptable>();

            rootVisualElement.Add(SatanAbilitiesEditor.CreateCachedEditorAndBind(_scriptable));
        }
    }
}