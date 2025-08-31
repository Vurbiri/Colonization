using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    sealed public partial class WarriorSkin : ActorSkin
    {
        private BlockState _blockState;

        public override void Init(Id<PlayerId> owner, Skills skills)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = GameContainer.Materials[owner].materialWarriors;

            var sfx = GetComponent<WarriorSFX>();

            base.Init(skills, sfx);

            _blockState = new("bBlock", this, sfx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public void Block(bool isActive)
        {
            if (isActive)
                _stateMachine.SetState(_blockState);
            _blockState.SfxEnable(isActive);
        }
    }
}
