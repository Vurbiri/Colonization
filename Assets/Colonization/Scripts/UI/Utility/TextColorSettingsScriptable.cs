//Assets\Colonization\Scripts\UI\Utility\TextColorSettingsScriptable.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "TextColorSettingsScriptable", menuName = "Vurbiri/Colonization/TextColorSettingsScriptable", order = 51)]
    sealed public class TextColorSettingsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private TextColorSettings _colors;

		public TextColorSettings Colors => _colors.Init();
    }
}
