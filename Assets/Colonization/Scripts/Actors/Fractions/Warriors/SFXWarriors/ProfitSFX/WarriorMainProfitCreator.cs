namespace Vurbiri.Colonization
{
    sealed public class WarriorMainProfitCreator : AParticleCreatorSFX
    {
        public override APooledSFX Create(System.Action<APooledSFX> deactivate) => new WarriorMainProfit(this, deactivate);

#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => TargetForSFX_Ed.Target;
#endif
    }
}
