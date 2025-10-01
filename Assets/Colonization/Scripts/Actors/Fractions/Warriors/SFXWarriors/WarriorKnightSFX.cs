namespace Vurbiri.Colonization.Actors
{
	sealed public class WarriorKnightSFX : AWarriorParticleSFX
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            SetProperties_Ed("LeftHand", "PS_Glow");
        }
#endif
    }
}
