using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    sealed public class ArtefactWindow : ABuffsWindow
    {
        private const string NAME = "Artefact", MENU = CONST_EDITOR.MENU_BUFFS_PATH + NAME;

        [MenuItem(MENU, false, 13)]
        private static void ShowWindow()
        {
            GetWindow<ArtefactWindow>(true, NAME).minSize = new(300f, 300f);
        }

        private void OnEnable()
        {
            _excludeAbility.Add(ActorAbilityId.MaxAP); _excludeAbility.Add(ActorAbilityId.APPerTurn);
            _excludeAbility.Add(ActorAbilityId.ProfitMain); _excludeAbility.Add(ActorAbilityId.ProfitAdv);
            
            base.Enable("ArtefactSettings", "Weight", 100);
        }
    }
}
