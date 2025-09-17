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
    }
}
