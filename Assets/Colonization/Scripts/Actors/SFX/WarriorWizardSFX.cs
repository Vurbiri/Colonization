//Assets\Colonization\Scripts\Actors\SFX\WarriorWizardSFX.cs
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorWizardSFX : AWarriorSFX
    {
        [SerializeField] private ParticleSystem[] _particles;
        [SerializeField] private MeshRenderer _magicShield;

        protected override IEnumerator Start()
        {
            _magicShield.enabled = false;

            yield return base.Start();

            for(int i = 0; i < _particles.Length; i++)
                _particles[i].Play();
        }

        public override void Block(bool isActive) { _magicShield.enabled = isActive; }

        public override IEnumerator Death_Coroutine()
        {
            for (int i = 0; i < _particles.Length; i++)
                _particles[i].Stop();

            yield return base.Death_Coroutine();
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_particles == null || _particles.Length == 0)
                _particles = GetComponentsInChildren<ParticleSystem>();

            if(_magicShield == null)
                _magicShield = GetComponentInChildren<MeshRenderer>();
        }
#endif
    }
}
