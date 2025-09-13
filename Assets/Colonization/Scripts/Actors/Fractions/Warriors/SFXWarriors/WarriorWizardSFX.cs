using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorWizardSFX : WarriorSFX
    {
        [Space]
        [SerializeField] private Transform _rightHand;
        [SerializeField] private MeshRenderer _magicShield;
        [SerializeField] private ReadOnlyArray<ParticleSystem> _particles;

        public override Transform TargetTransform => _rightHand;

        protected override IEnumerator Start()
        {
            _magicShield.enabled = false;

            yield return base.Start();

            for(int i = 0; i < _particles.Count; i++)
                _particles[i].Play();
        }

        public override void Block(bool isActive) { _magicShield.enabled = isActive; }

        public override void Death()
        {
            _magicShield.enabled = false;
            for (int i = 0; i < _particles.Count; i++)
                _particles[i].Stop();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _rightHand, "RightHand");
            this.SetChildren(ref _magicShield);

            if (_particles == null || _particles.Count == 0)
                _particles = new(GetComponentsInChildren<ParticleSystem>());
        }
#endif
    }
}
