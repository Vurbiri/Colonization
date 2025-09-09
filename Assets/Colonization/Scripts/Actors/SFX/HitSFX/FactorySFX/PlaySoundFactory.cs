using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = FILE_NAME, menuName = MENU + "Sound", order = ORDER)]
    public class PlaySoundFactory : ASFXFactory
    {
        private enum TypeSound { Impact, Target, User }

        [SerializeField] private AudioClip _clip;
        [Space]
        [SerializeField] private TypeSound _type;
        [SerializeField] private bool _isWait;

        public override IHitSFX Create() => _type switch
        {
            TypeSound.Impact => _isWait ? new WaitImpactSound(_clip)   : new ImpactSound(_clip),
            TypeSound.Target => _isWait ? new WaitSoundOnTarget(_clip) : new SoundOnTarget(_clip),
            TypeSound.User   => _isWait ? new WaitSoundOnUser(_clip)   : new SoundOnUser(_clip),
            _ => null
        };


#if UNITY_EDITOR
        public override TargetForSFX_Ed Target_Ed => _type == TypeSound.User ? TargetForSFX_Ed.User : TargetForSFX_Ed.Target;
#endif
    }
}
