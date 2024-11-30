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
            protected readonly IReadOnlyList<EffectsHint> _effectsHint;
            protected readonly int _countPackets;

            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public ASkillState(Actor parent, IReadOnlyList<EffectsHint> effects, int cost, int id) : base(parent, cost, TypeIdKey.Get<ASkillState>(id))
            {
                _id = id;
                _parentTransform = _actor._thisTransform;
                _effectsHint = effects;
                _countPackets = _effectsHint.Count;
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

            protected abstract IEnumerator Actions_Coroutine();

            protected IEnumerator ApplySkill_Coroutine(Transform target)
            {
                WaitActivate wait = _skin.Skill(_id, target);

                for (int i = 0; i < _countPackets; i++)
                {
                    yield return wait;
                    Hint(i);
                    wait.Reset();
                }
                Pay();
                yield return wait;
            }

            protected abstract void Hint(int index);
        }
    }
}
