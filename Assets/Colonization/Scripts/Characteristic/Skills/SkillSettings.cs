namespace Vurbiri.Colonization
{
    using Vurbiri.Colonization.Actors;
    using Vurbiri.Colonization.UI;
    using static Actors.Actor;

    public partial class Skills
    {
        [System.Serializable]
        public class SkillSettings
        {
            public AnimationClipSettingsScriptable clipSettings;
            public bool isMove;
            public bool isValid;
            public int percentDamage;
            public float range;
            public ASkillState.Settings settings;
            public SkillUI ui;
        }
    }
}
