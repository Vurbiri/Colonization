using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = MENU + "Sound", order = ORDER)]
    public class PlaySoundFactory : ASFXFactory
    {
        [Space]
        [SerializeField] private SFXType _type;
        [SerializeField] private bool _isWait;
        [Space]
        [SerializeField] private AudioClip _clip;

        public override ISFX Create() => _type switch
        {
            SFXType.Impact => _isWait ? new WaitImpactSound(_clip)   : new ImpactSound(_clip),
            SFXType.Target => _isWait ? new WaitSoundOnTarget(_clip) : new SoundOnTarget(_clip),
            SFXType.User   => _isWait ? new WaitSoundOnUser(_clip)   : new SoundOnUser(_clip),
            _ => null
        };


#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => _type == SFXType.User ? TargetForSFX_Ed.User : TargetForSFX_Ed.Target;
#endif
    }
}
