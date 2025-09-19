using UnityEditor;
using Vurbiri.Colonization.Characteristics;
using static VurbiriEditor.Colonization.CONST_EDITOR;

namespace VurbiriEditor.Colonization
{
    sealed public class ArtefactWindow : ABuffsWindow
    {
        private const string NAME = "Artefact", MENU = MENU_BUFFS_PATH + NAME;

        [MenuItem(MENU, false, MENU_BUFFS_ORDER)]
        private static void ShowWindow()
        {
            GetWindow<ArtefactWindow>(true, NAME).minSize = new(300f, 300f);
        }

        private void OnEnable()
        {
            _excludeAbility.Add(ActorAbilityId.MaxHP);
            _excludeAbility.Add(ActorAbilityId.MaxAP); _excludeAbility.Add(ActorAbilityId.APPerTurn);
            _excludeAbility.Add(ActorAbilityId.ProfitMain); _excludeAbility.Add(ActorAbilityId.ProfitAdv);
            
            base.Enable("ArtefactSettings", "Weight", 100);
        }

        private void OnDisable()
        {
            base.Disable(true);
        }
    }
}
