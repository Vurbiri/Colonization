using UnityEngine;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class SceneColorsEd
	{
		[Header("┌──────────── Panel ─────────────────────")]
		public Color panelBack;
		public Color panelText;
		[Header("├──────────── Hint ─────────────────────")]
		public Color hintBack;
		public Color hintText;
		[Header("├──────────── Text ─────────────────────")]
		public Color textDark;
        public Color textLight = Color.white;
        [Header("├──────────── Other ─────────────────────")]
        public Color elements;
        public Color menu;

        public SceneColorsEd Clone() => new ()
		{
				panelBack = panelBack,
				panelText = panelText,
				hintBack = hintBack,
				hintText = hintText,
				textDark = textDark,
				elements = elements,
				menu = menu
		};
}
}
