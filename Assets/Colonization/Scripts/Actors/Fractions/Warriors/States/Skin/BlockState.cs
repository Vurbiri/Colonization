using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class WarriorSkin
    {
        private static readonly int s_idBlock = Animator.StringToHash("bBlock");

        sealed private class BlockState : BoolSwitchState
        {
            private readonly WarriorSFX _sfx;
            private readonly WaitScaledTime _wait;

            public BlockState(WarriorSkin parent, WarriorSFX sfx, WaitScaledTime wait) : base(s_idBlock, parent)
            {
                _sfx = sfx;
                _wait = wait;
            }

            public Enumerator Enable(bool enable)
            {
                _sfx.Block(enable);
                return enable ? _wait.Restart() : null;
            }
        }
    }
}
