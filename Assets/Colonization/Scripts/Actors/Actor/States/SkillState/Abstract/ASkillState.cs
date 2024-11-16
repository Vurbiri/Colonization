namespace Vurbiri.Colonization.Actors
{
    using Characteristics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract partial class Actor
	{
		public abstract partial class ASkillState : AState
        {
            protected readonly int _idAnimation;
            protected readonly int _cost;
            protected readonly IReadOnlyList<AEffect> _effects;
            protected readonly Transform _parentTransform;
            
            protected Actor _target;
            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public ASkillState(Actor parent, IReadOnlyList<AEffect> effects, Settings settings, int id) : base(parent, id)
            {
                _idAnimation = settings.idAnimation;
                _cost = settings.cost;
                _effects = effects;
                _parentTransform = _actor._thisTransform;
                _waitTargetSkillAnimation = new(settings.damageTime);
                _waitEndSkillAnimation = new(settings.remainingTime);
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

                _target = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                _eventBus.TriggerActorUnselect(_actor);
            }

            protected void Reset()
            {
                _coroutineAction = null;
                _fsm.Default();
            }

            protected abstract IEnumerator Actions_Coroutine();

            protected IEnumerator ApplySkill_Coroutine()
            {
                _skin.Skill(_idAnimation);
                yield return _waitTargetSkillAnimation;
                yield return _waitEndSkillAnimation;
            }

            #region Nested: Settings
            //*******************************************************
            [System.Serializable]
            public class Settings
            {
                public int targetActor;
                public float damageTime;
                public float remainingTime;
                public int idAnimation;
                public int cost;
            }
            #endregion
        }
    }
}