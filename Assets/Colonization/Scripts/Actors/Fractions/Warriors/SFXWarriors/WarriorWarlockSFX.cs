namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorWarlockSFX : AWarriorParticleSFX
    {
#if UNITY_EDITOR
        private void OnValidate() => SetProperties_Ed("RightHand", "PS_Flame");
#endif
    }
}
