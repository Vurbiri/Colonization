using System.Collections;

namespace Vurbiri.Colonization
{
	public class WaitComboSFX : ISFX
    {
        private readonly string _nameUser, _nameTarget;
        private readonly WaitRealtime _delay;
        private readonly WaitAllEnumerators _waitAll = new();

        public WaitComboSFX(string nameUser, string nameTarget, float delayTime)
        {
            _nameUser = nameUser;
            _nameTarget = nameTarget;
            _delay = delayTime;
        }

        public IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            _waitAll.Clear();
            _waitAll.Add(GameContainer.SFX.Run(_nameUser, user, target)); 
            yield return _delay;
            yield return _waitAll.Add(GameContainer.SFX.Run(_nameTarget, user, target));
        }
    }
}
