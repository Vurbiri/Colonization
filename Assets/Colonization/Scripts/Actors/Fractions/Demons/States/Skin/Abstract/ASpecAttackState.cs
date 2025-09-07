namespace Vurbiri.Colonization.Actors
{
    public partial class ADemonSkin
    {
        protected abstract class ASpecAttackState : ASpecState
        {
            public ASpecAttackState(ActorSkin parent, DemonSFX sfx) : base(s_idSpecSkill, parent, sfx) { }
        }
    }
}
