//Assets\Colonization\Scripts\Utility\TextColorSettingsScriptable.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    //[CreateAssetMenu(fileName = "TextColorSettingsScriptable", menuName = "Vurbiri/Colonization/TextColorSettingsScriptable", order = 51)]
    public class TextColorSettingsScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private TextColorSettings _colors;

		public TextColorSettings Colors => _colors.Init();
    }
}
