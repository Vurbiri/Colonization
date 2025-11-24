using System;
using System.Collections;
using static UnityEngine.ParticleSystem;

namespace Vurbiri.Colonization
{
    sealed public class WarriorMainProfit : AParticleOnTarget
    {
        private MainModule _mainModule;
        private EmissionModule _emissionModule;
        private TextureSheetAnimationModule _animationModule;
        private Burst _burst;
        private readonly float _lifeTime;

        public WarriorMainProfit(WarriorMainProfitCreator creator, Action<APooledSFX> deactivate) : base(creator, deactivate)
        {
            _mainModule = _particle.main;
            _emissionModule = _particle.emission;
            _animationModule = _particle.textureSheetAnimation;

            _burst = _emissionModule.GetBurst(0);
            _lifeTime = _mainModule.startLifetime.constant;
        }

        public override IEnumerator Run(ActorSFX user, ActorSkin target)
        {
            var actor = target.GetComponentInParent<Actor>();
            int count = GameContainer.Humans[actor.Owner].Abilities[HumanAbilityId.WarriorProfit].Value;

            _mainModule.duration = _lifeTime + _burst.repeatInterval * (count + 1);

            _burst.cycleCount = count;
            _emissionModule.SetBurst(0, _burst);

            _animationModule.rowIndex = actor.Hexagon.GetProfit().Value;

            Setup(target);
            return this;
        }
    }
}
