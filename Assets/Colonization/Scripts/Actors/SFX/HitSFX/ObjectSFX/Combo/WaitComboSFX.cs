using System.Collections;

namespace Vurbiri.Colonization.Actors
{
	public class WaitComboSFX : IHitSFX
    {
        private readonly string _nameUser, _nameTarget;
        private readonly WaitRealtime _delay;
        private readonly WaitAll _waitAll = new();

        public WaitComboSFX(string nameUser, string nameTarget, float delayTime)
        {
            _nameUser = nameUser;
            _nameTarget = nameTarget;
            _delay = delayTime;
        }

        public IEnumerator Hit(ActorSFX user, ActorSkin target)
        {
            _waitAll.Clear();
            var userSFX = GameContainer.HitSFX.Hit(_nameUser, user, target);
            yield return _delay;
            yield return _waitAll.Add(userSFX, GameContainer.HitSFX.Hit(_nameTarget, user, target));
        }
    }
}
