namespace Vurbiri.Colonization.Actors
{
	sealed public class WarriorKnightSFX : AWarriorParticleSFX
    {

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetProperties_Ed("LeftHand", "PS_Glow");
        }
#endif
    }
}
