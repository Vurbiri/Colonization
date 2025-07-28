using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorWizardSFX : WarriorSFX
    {
        [Space]
        [SerializeField] private Transform _rightHand;
        [SerializeField] private MeshRenderer _magicShield;
        [SerializeField] private ParticleSystem[] _particles;

        public override Transform StartTransform => _rightHand;

        protected override IEnumerator Start()
        {
            _magicShield.enabled = false;

            yield return base.Start();

            for(int i = 0; i < _particles.Length; i++)
                _particles[i].Play();
        }

        public override void Block(bool isActive) { _magicShield.enabled = isActive; }

        public override void Death()
        {
            _magicShield.enabled = false;
            for (int i = 0; i < _particles.Length; i++)
                _particles[i].Stop();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _rightHand, "RightHand");

            if (_particles == null || _particles.Length == 0)
                _particles = GetComponentsInChildren<ParticleSystem>();

            if(_magicShield == null)
                _magicShield = GetComponentInChildren<MeshRenderer>();
        }
#endif
    }
}
