using UnityEditor;
using Vurbiri.Colonization.Characteristics;

namespace VurbiriEditor.Colonization
{
    public class BuffsWindow : ABuffsWindow<BuffSettings>
    {
        #region Consts
        private const string NAME = "Artefacts", MENU = CONST_EDITOR.MENU_BUFFS_PATH + NAME;
        #endregion

        [MenuItem(MENU, false, 13)]
        private static void ShowWindow()
        {
            GetWindow<BuffsWindow>(true, NAME).minSize = new(300f, 300f);
        }

        protected override void OnEnable()
        {
            _excludeAbility.Add(ActorAbilityId.MaxAP); _excludeAbility.Add(ActorAbilityId.APPerTurn);
            _excludeAbility.Add(ActorAbilityId.ProfitMain); _excludeAbility.Add(ActorAbilityId.ProfitAdv);
            base.OnEnable();
        }
    }
}
