namespace Vurbiri.Colonization.Actors
{
    public partial class WarriorSkin
    {
        sealed private class BlockState : ASkinState
        {
            private readonly WarriorSFX _sfx;

            public BlockState(string stateName, WarriorSkin parent, WarriorSFX sfx) : base(stateName, parent)
            {
                _sfx = sfx;
            }

            public override void Enter() => EnableAnimation();

            public override void Exit() => DisableAnimation();

            public void SfxEnable(bool enable) => _sfx.Block(enable);
        }
    }
}
