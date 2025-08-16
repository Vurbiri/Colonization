using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    sealed public class WarriorWarlockSFX : WarriorSFX
    {
        [Space]
        [SerializeField] private Transform _rightHand;
        [SerializeField] private ParticleSystem _particle;

        public override Vector3 StartPosition => _rightHand.position;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            _particle.Play();
        }

        public override void Death()
        {
            _particle.Stop();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _rightHand, "RightHand");
            this.SetChildren(ref _particle, "PS_Flame");
        }
#endif
    }
}
