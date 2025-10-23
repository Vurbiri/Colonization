using UnityEngine;

namespace Vurbiri.Colonization
{
	public abstract partial class ADemonSkin : ActorSkin
    {
        protected static readonly int s_idSpecMove = Animator.StringToHash("bWalk_Spec"), s_idSpecSkill = Animator.StringToHash("bSkill_Spec");
    }
}
