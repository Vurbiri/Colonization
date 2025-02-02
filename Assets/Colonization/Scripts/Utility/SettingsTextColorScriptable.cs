//Assets\Colonization\Scripts\Utility\SettingsTextColorScriptable.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "SettingsTextColorScriptable", menuName = "Vurbiri/Colonization/SettingsTextColorScriptable", order = 51)]
    public class SettingsTextColorScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private SettingsTextColor _colors;

		public SettingsTextColor Colors => _colors.Init();
    }
}
