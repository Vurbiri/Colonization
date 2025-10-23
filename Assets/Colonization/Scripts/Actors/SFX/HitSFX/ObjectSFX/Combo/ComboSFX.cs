using System.Collections;

namespace Vurbiri.Colonization
{
    public class ComboSFX : ISFX
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

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            RunInternal(user, target).Start();
            yield break;
        }

        private IEnumerator RunInternal(ActorSFX user, ActorSkin target)
        {
            GameContainer.SFX.Run(_nameUser, user, target).Start();
            yield return _delay;
            yield return GameContainer.SFX.Run(_nameTarget, user, target);
        }
    }
}
