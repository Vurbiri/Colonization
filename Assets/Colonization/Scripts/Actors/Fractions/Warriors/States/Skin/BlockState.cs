namespace Vurbiri.Colonization.Actors
{
    public partial class WarriorSkin
    {
        sealed private class BlockState : BoolSwitchState
        {
            private readonly WarriorSFX _sfx;

            public BlockState(WarriorSkin parent, WarriorSFX sfx) : base("bBlock", parent)
            {
                _sfx = sfx;
            }

            public void SfxEnable(bool enable) => _sfx.Block(enable);
        }
    }
}
