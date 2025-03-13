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
		

        [MenuItem(MENU)]
		private static void ShowWindow()
		{
			GetWindow<DemonBuffsWindow>(true, NAME).minSize = new(300f, 600f); ;
		}

        protected override void OnEnable()
        {
            _values.Add(1); _names.Add("Addition");
            base.OnEnable();
        }

        protected override void DrawValues(DemonBuffSettings settings)
        {
           base.DrawValues(settings);
            settings.levelUP = EditorGUILayout.IntSlider("Level Up", settings.levelUP, 1, 25);
        }
    }
}
