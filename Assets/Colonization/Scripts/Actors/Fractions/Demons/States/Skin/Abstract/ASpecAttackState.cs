namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected abstract class ASpecAttackState : ASpecState
        {
            public ASpecAttackState(ActorSkin parent, DemonSFX sfx) : base("bSkill_Spec", parent, sfx) { }
        }
    }
}
