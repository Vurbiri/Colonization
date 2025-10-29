using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public partial class WarriorSkin : ActorSkin
    {
        private BlockState _blockState;

        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = GameContainer.Materials[owner].materialWarriors;

            var sfx = GetComponent<WarriorSFX>();
            sfx.Init(skills.HitSfxNames);

            base.InitInternal(skills.Timings, sfx);

            _blockState = new(this, sfx, skills.Spec.Timing.WaitEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public Enumerator Block(bool isActive)
        {
            if (isActive)
                _stateMachine.SetState(_blockState);
            return _blockState.Enable(isActive);
        }
    }
}
