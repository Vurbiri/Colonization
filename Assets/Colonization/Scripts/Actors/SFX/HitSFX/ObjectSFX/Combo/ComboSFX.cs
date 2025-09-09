using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public class ComboSFX : IHitSFX
    {
        private readonly string _nameUser, _nameTarget;
        private readonly WaitRealtime _delay;

        public ComboSFX(string nameUser, string nameTarget, float delayTime)
        {
            _nameUser = nameUser;
            _nameTarget = nameTarget;
            if (delayTime > 0f)
                _delay = delayTime;
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            HitInternal(user, target).Start();
            return null;
        }

        private IEnumerator HitInternal(ActorSFX user, ActorSkin target)
        {
            GameContainer.HitSFX.Hit(_nameUser, user, target);
            yield return _delay;
            GameContainer.HitSFX.Hit(_nameTarget, user, target);
        }
    }
}
