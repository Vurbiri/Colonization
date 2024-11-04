using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    using static Actors.Actor;

    public partial class Skills
    {
        [System.Serializable]
        public class AttackSkill
        {
            public AnimationClipSettingsScriptable clipSettings;
            public bool isValid;
            public AttackState.Settings settings;
            public AttackSkillUI ui;
        }
    }
}
