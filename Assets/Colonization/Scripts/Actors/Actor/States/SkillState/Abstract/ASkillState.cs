//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\Abstract\ASkillState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
	{
		public abstract partial class ASkillState : AActionState
        {
            protected readonly int _id;
            protected readonly Transform _parentTransform;
            protected readonly IReadOnlyList<EffectsHit> _effectsHint;
            protected readonly int _countHits;

            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public ASkillState(Actor parent, IReadOnlyList<EffectsHit> effects, int cost, int id) : base(parent, cost, TypeIdKey.Get<ASkillState>(id))
            {
                _id = id;
                _parentTransform = _actor._thisTransform;
                _effectsHint = effects;
                _countHits = _effectsHint.Count;
            }

            public override void Enter()
            {
                _coroutineAction = _actor.StartCoroutine(Actions_Coroutine());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _actor.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }
            }

            protected void ToExit()
            {
                _coroutineAction = null;
                _fsm.ToDefaultState();
            }

            protected override void Pay()
            {
                base.Pay();
                _actor.TriggerChange();
            }

            protected abstract IEnumerator Actions_Coroutine();
            protected abstract IEnumerator ApplySkill_Coroutine();
        }
    }
}
