using UnityEditor;
using Vurbiri.Colonization;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
	public class WarriorsAISettingsWindow : ActorAISettingsWindow<WarriorsAISettings, WarriorId, WarriorAIStateId>
    {
        private const string NAME = "Warriors", MENU = MENU_AA_PATH + NAME;

        [MenuItem(MENU, false, MENU_AI_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<WarriorsAISettingsWindow>(true, NAME).minSize = s_minSize;
        }

        protected override ActorsAISettingsDrawer Init(SerializedProperty property, out string label)
        {
            label = "WarriorAI Settings";
            return new(ActorTypeId.Warrior, WarriorId.Count, WarriorId.Names_Ed, property);
        }
    }
}
