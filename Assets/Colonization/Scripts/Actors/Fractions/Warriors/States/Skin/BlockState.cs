using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class WarriorSkin
    {
        private static readonly int s_idBlock = Animator.StringToHash("bBlock");

        sealed private class BlockState : BoolSwitchState
        {
            private readonly WarriorSFX _sfx;

            public BlockState(WarriorSkin parent, WarriorSFX sfx) : base(s_idBlock, parent)
            {
                _sfx = sfx;
            }

            public void SfxEnable(bool enable) => _sfx.Block(enable);
        }
    }
}
