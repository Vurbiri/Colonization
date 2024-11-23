//Assets\Colonization\Scripts\Characteristics\Skills\SkillSettings.cs
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
            public TargetOfSkill target;
            public bool isMove;
            public bool isTargetReact;
            public float range;
            public EffectSettings[] effects;
            public Actor.ASkillState.Settings settings;
            public SkillUI ui;

            public void SetTiming()
            {
                settings.damageTime = clipSettings.damageTime;
                settings.remainingTime = clipSettings.RemainingTime;
                range = clipSettings.range;
            }
        }
    }
}
