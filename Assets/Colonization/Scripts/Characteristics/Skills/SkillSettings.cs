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
            public bool isMove;
            public bool isTargetReact;
            public float range;
            public int cost;
            public EffectsPacketSettings[] effectsPacket;
            public SkillUI ui;
#if UNITY_EDITOR
            public AnimationClipSettingsScriptable clipSettings;
#endif
        }
    }
}
