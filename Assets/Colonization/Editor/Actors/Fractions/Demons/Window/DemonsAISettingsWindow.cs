using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class DemonsAISettingsWindow : ActorAISettingsWindow<DemonsAISettings, DemonId, DemonAIStateId>
    {
        private const string NAME = "Demons", MENU = MENU_AA_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<DemonsAISettingsWindow>(true, NAME).minSize = s_minSize;
        }

        protected override string Label => "DemonAI Settings";

        protected override ActorsAISettingsDrawer GetSettingsDrawer(SerializedProperty property)
        {
            return new(ActorTypeId.Demon, DemonId.Count, DemonId.Names_Ed, property);
        }
    }
}
