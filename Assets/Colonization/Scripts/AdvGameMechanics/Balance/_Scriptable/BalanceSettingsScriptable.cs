//Assets\Colonization\Scripts\AdvGameMechanics\Balance\_Scriptable\BalanceSettingsScriptable.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "BalanceSettings", menuName = "Vurbiri/Colonization/Balance Settings", order = 51)]
    sealed public class BalanceSettingsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private BalanceSettings _settings;

        public static implicit operator BalanceSettings(BalanceSettingsScriptable self) => self._settings;
    }
}
