//Assets\Colonization\Editor\Characteristic\Buffs\DemonBuffsWindow.cs
using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    public class DemonBuffsWindow : ABuffsWindow<DemonBuffSettings>
    {
		#region Consts
		private const string NAME = "Demon Buffs", MENU = CONST_EDITOR.MENU_BUFFS_PATH + NAME;
        #endregion


        [MenuItem(MENU, false, 12)]
        private static void ShowWindow()
		{
			GetWindow<DemonBuffsWindow>(true, NAME).minSize = new(300f, 500f); ;
		}

        protected override void OnEnable()
        {
            _values.Add(1); _names.Add("Addition");
            base.OnEnable();
        }

        protected override bool DrawValues(DemonBuffSettings settings)
        {
            int oldLevelUP = settings.levelUP;
            
            bool isSave = base.DrawValues(settings);
            settings.levelUP = EditorGUILayout.IntSlider("Level Up", settings.levelUP, 1, 25);

            return isSave | oldLevelUP != settings.levelUP;
        }
    }
}
