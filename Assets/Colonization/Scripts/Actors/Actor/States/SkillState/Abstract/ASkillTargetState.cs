//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\Abstract\ASkillTargetState.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract class ASkillTargetState : ASkillState
        {
            protected Actor _target;
            protected bool _isTargetReact = true;
            protected WaitActivate _waitActor;
            protected readonly Relation _relationTarget;
            protected readonly Transform _parentTransform;

            protected ASkillTargetState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<AEffect> effects, Settings settings, int id) : 
                base(parent, effects, settings, id)
            {
                _parentTransform = _actor._thisTransform;

                _relationTarget = targetActor.ToRelation();
                _relationTarget = Relation.Friend;
            }

            public override void Exit()
            {
                base.Exit();

                if (_target != null)
                    _target._stateMachine.ToPrevState();

                _waitActor = null;
                _target = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                if (_waitActor == null)
                    return;

                if (newSelectable is Hexagon hex)
                    _target = CheckTarget(hex.Owner);
                else
                    _target = CheckTarget(newSelectable as Actor);

                _waitActor.Activate();
            }

            protected IEnumerator SelectActor_Coroutine(Action<bool> callback)
            {
                Hexagon currentHex = _actor._currentHex;
                List<Hexagon> targets = new(HEX_COUNT_SIDES);

                foreach (var hex in currentHex.Neighbors)
                    if (hex.TrySetSelectableActor(_actor._owner, _relationTarget))
                        targets.Add(hex);

                if (targets.Count == 0)
                    yield break;

                yield return _waitActor = new();

                foreach (var hex in targets)
                    hex.SetUnselectable();

                if (_target == null)
                    yield break;

                Hexagon targetHex = _target._currentHex;
                _parentTransform.localRotation = ACTOR_ROTATIONS[targetHex.Key - currentHex.Key];
                if(_isTargetReact)
                    _target._thisTransform.localRotation = ACTOR_ROTATIONS[currentHex.Key - targetHex.Key];

                callback(true);
            }

            protected override IEnumerator ApplySkill_Coroutine()
            {
                _skin.Skill(_idAnimation);
                yield return _waitTargetSkillAnimation;

                for (int i = 0; i < _countEffects; i++)
                    _effects[i].Apply(_actor, _target);

                Pay();
                _target.ReactionToAttack(_isTargetReact);

                yield return _waitEndSkillAnimation;
                
            }

            private Actor CheckTarget(Actor target)
            {
                if (target == null)
                    return null;

                Key key = target._currentHex.Key - _actor._currentHex.Key;

                if (target == _actor || target.GetRelation(_actor._owner) != _relationTarget || !ACTOR_ROTATIONS.ContainsKey(key))
                    return null;

                target.BecomeTarget(_actor._owner, _relationTarget);
                return target;
            }
        }
    }
}
