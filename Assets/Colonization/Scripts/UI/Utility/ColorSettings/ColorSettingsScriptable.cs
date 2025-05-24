using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "ColorSettingsScriptable", menuName = "Vurbiri/Colonization/ColorSettingsScriptable", order = 51)]

    sealed public class ColorSettingsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private ProjectColors _colors;

		public ProjectColors Colors => _colors.Init();
    }
}
