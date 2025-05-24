using System;
using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    public class DemonBuffsWindow : ABuffsWindow<DemonBuffSettings>
    {
		#region Consts
		private const string NAME = "Demon Buffs", MENU = CONST_EDITOR.MENU_BUFFS_PATH + NAME;
        private const int MIN_LEVEL = 1, MAX_LEVEL = 25;
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
            settings.levelUP = Math.Clamp(settings.levelUP, MIN_LEVEL, MAX_LEVEL);
            settings.levelUP = EditorGUILayout.IntSlider("Level Up", settings.levelUP, MIN_LEVEL, MAX_LEVEL);

            return isSave | oldLevelUP != settings.levelUP;
        }
    }
}
