namespace Vurbiri.Colonization.Characteristics
{
    using Actors;
    using UI;

    public partial class Skills
    {
        [System.Serializable]
        public class SkillSettings
        {
            public AnimationClipSettingsScriptable clipSettings;
            public int target;
            public bool isMove;
            public bool isValid;
            public float range;
            public EffectSettings[] effects;
            public Actor.ASkillState.Settings settings;
            public SkillUI ui;
        }
    }
}
