namespace Vurbiri.Colonization.Actors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using static CONST;

    public abstract partial class Actor
	{
		public abstract class ASkillState : AState
        {
            protected readonly int _percentDamage;
            protected readonly int _idAnimation;
            protected readonly int _cost;
            protected readonly List<Effect> _selfEffects;
            protected readonly Transform _parentTransform;
            protected WaitActivate _waitActor;
            protected Hexagon _targetActor;
            protected Coroutine _coroutineAction;
            protected readonly WaitForSeconds _waitTargetSkillAnimation, _waitEndSkillAnimation;

            public ASkillState(Actor parent, int percentDamage, Settings settings, int id) : base(parent, id)
            {
                _percentDamage = percentDamage;
                _idAnimation = settings.idAnimation;
                _cost = settings.cost;

                _parentTransform = _parent._thisTransform;
                _waitTargetSkillAnimation = new(settings.damageTime);
                _waitEndSkillAnimation = new(settings.remainingTime);
            }

            public override void Enter()
            {
                _coroutineAction = _parent.StartCoroutine(Actions_Coroutine());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _parent.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }

                _waitActor = null;
                _targetActor = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                _eventBus.TriggerActorUnselect(_parent);

                if (_waitActor == null)
                    return;

                _targetActor = newSelectable as Hexagon;
                _waitActor.Activate();
            }

            protected void Reset()
            {
                _coroutineAction = null;
                _fsm.Default();
            }

            protected abstract IEnumerator Actions_Coroutine();

            protected IEnumerator SelectActor_Coroutine(Action<bool> callback)
            {
                Hexagon currentHex = _parent._currentHex;
                List<Hexagon> empty = new(6);

                foreach (var hex in currentHex.Neighbors)
                    if (hex.CanUnitEnter)
                        empty.Add(hex);

                if (empty.Count == 0)
                    yield break;


                _waitActor = new();

                foreach (var hex in empty)
                    hex.TrySetSelectable(false);

                yield return _waitActor;

                foreach (var hex in empty)
                    hex.SetUnselectable();

                if (_targetActor == null)
                    yield break;

                _parentTransform.localRotation = ACTOR_ROTATIONS[_targetActor.Key - currentHex.Key];
                
                callback(true);
            }

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
                public float damageTime;
                public float remainingTime;
                public int idAnimation;
                public int cost;
                public EffectSettings[] effects;
            }
            #endregion
        }
    }
}
