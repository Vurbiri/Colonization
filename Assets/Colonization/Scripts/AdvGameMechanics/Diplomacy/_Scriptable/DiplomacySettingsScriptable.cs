//Assets\Colonization\Scripts\Diplomacy\_Scriptable\DiplomacySettingsScriptable.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "DiplomacySettings", menuName = "Vurbiri/Colonization/Diplomacy Settings", order = 51)]
    sealed public class DiplomacySettingsScriptable : ScriptableObjectDisposable
	{
		[SerializeField] private DiplomacySettings _settings;

		public static implicit operator DiplomacySettings(DiplomacySettingsScriptable self) => self._settings;
    }
}
