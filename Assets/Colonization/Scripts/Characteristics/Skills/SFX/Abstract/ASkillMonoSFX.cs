//Assets\Colonization\Scripts\Characteristics\Skills\SFX\Abstract\ASkillSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization.Characteristics
{
    public abstract class ASkillMonoSFX : MonoBehaviour, ISkillSFX
    {
        protected GameObject _thisGO;
        protected Transform _thisTransform, _parent;

        protected virtual void Awake()
        {
            _thisGO = gameObject;
            _thisGO.SetActive(false);

            _thisTransform = transform;
            _parent = _thisTransform.parent;
        }

        public abstract void Run(Transform target);

        public virtual void Hint(int index) { }

        public abstract ISkillSFX Init(IActorSFX parent, float duration);
    }
}
