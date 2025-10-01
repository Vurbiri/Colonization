namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorWarlockSFX : AWarriorParticleSFX
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            SetProperties_Ed("RightHand", "PS_Flame");
        }
#endif
    }
}
