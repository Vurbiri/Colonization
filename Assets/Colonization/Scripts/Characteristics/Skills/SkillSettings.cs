using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
{
    public partial class Skills
    {
        [System.Serializable]
        public class SkillSettings
        {
            public AnimationClipSettingsScriptable clipSettings;
            public int target;
            public bool isMove;
            public bool isTargetReact;
            public float range;
            public EffectSettings[] effects;
            public Actor.ASkillState.Settings settings;
            public SkillUI ui;
        }
    }
}
