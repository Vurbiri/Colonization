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
            public TargetOfSkill target;
            public float range;
            public float distance;
            public int cost;
            public EffectsHitSettings[] effectsHits;
            public SkillUI ui;

#if UNITY_EDITOR
            public AHitScriptableSFX[] SFXHits;
            public AnimationClipSettingsScriptable clipSettings;
#endif
        }
    }
}
