namespace Vurbiri.Colonization
{
    using Vurbiri.Colonization.Actors;
    using Vurbiri.Colonization.UI;
    using static Actors.Actor;

    public partial class Skills
    {
        [System.Serializable]
        public class AttackSkill
        {
            public AnimationClipSettingsScriptable clipSettings;
            public bool isValid;
            public int percentDamage;
            public AttackState.Settings settings;
            public AttackSkillUI ui;
        }
    }
}
