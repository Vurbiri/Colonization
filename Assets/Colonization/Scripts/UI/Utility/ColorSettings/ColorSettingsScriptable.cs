using UnityEngine;
using VurbiriEditor.Colonization;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "ColorSettingsScriptable", menuName = "Vurbiri/Colonization/ColorSettingsScriptable", order = 51)]

    sealed public class ColorSettingsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private ProjectColors _colors;

		public ProjectColors Colors => _colors.Init();

#if UNITY_EDITOR
        public void SetColors_Editor(UISettings_Editor.Colors colors)
        {
            _colors.SetColors_Editor(colors);
        }
#endif
    }
}
