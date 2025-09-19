namespace Vurbiri.Colonization.Actors
{
    public enum SFXType 
    { 
        Impact, 
        Target, 
        User 
    }

#if UNITY_EDITOR
    public enum TargetForSFX_Ed
    {
        None = -1,
        Target,
        User,
        Combo
    }
#endif
}

