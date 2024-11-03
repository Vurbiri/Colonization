using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    public partial class Skills
    {
        [System.Serializable]
        private class AttackSkill
        {
            public AnimationClip clip;
            
            public float totalTime;
            public float range;
            public float damageTime;
            public float percentDamage;


            public AttackSkillUI ui;
        }
    }
}
