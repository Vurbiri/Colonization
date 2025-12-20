using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "ColorSettingsScriptable", menuName = "Vurbiri/Colonization/ColorSettingsScriptable", order = 51)]

    sealed public class ColorSettingsScriptable : ScriptableObject
    {
        [SerializeField] private ProjectColors _colors;

		public ProjectColors Colors => _colors.Init();

        public static implicit operator ProjectColors(ColorSettingsScriptable self)
        {
            Resources.UnloadAsset(self);
            return self._colors.Init();
        }

#if UNITY_EDITOR
        [StartEditor, Space]
        [MinMax(1f, 2f)] public RefFloat windowsPixelsPerUnit = new(1.6f);
        [Space]
        public SceneColorsEd menu;
        [Space]
        public SceneColorsEd game;
        [EndEditor] public bool endEditor;

        private void OnValidate()
        {
            _colors.SetHintTextTag(game.hintText);
        }
#endif
    }
}
