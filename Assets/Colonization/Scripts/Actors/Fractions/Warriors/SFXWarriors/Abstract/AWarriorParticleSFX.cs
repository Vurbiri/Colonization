using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AWarriorParticleSFX : WarriorSFX
    {
        [Space]
        [SerializeField] private Transform _bone;
        [SerializeField] private ParticleSystem _particle;

        public override Vector3 Position => _bone.position;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            _particle.Play();
        }

        public override void Death() => _particle.Stop();

#if UNITY_EDITOR
        protected void SetProperties_Ed(string nameBone, string nameParticle)
        {
            this.SetChildren(ref _bone, nameBone);
            this.SetChildren(ref _particle, nameParticle);
        }
#endif
    }
}
