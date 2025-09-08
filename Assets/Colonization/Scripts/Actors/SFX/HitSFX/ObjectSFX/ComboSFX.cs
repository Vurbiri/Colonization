using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public class ComboSFX : IHitSFX
    {
        private readonly string _nameUser, _nameTarget;
        private readonly WaitRealtime _delay;
        private readonly WaitAll _waitAll = new();

        public ComboSFX(string nameUser, string nameTarget, WaitRealtime delay)
        {
            _nameUser = nameUser;
            _nameTarget = nameTarget;
            _delay = delay;
        }

        public IEnumerator Hit(ISFXUser user, ActorSkin target)
        {
            _waitAll.Clear();
            var userSFX = GameContainer.HitSFX.Hit(_nameUser, user, target);
            yield return _delay.Restart();
            yield return _waitAll.Add(userSFX, GameContainer.HitSFX.Hit(_nameTarget, user, target));
        }
    }
}
