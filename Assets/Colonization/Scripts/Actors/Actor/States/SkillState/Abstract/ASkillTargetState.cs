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
            protected Relation _targetActor;
            protected WaitActivate _waitActor;

            protected ASkillTargetState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<AEffect> effects, Settings settings, int id) : 
                base(parent, effects, settings, id)
            {
                _targetActor = targetActor.ToRelation();
                _targetActor = Relation.Friend;
            }

            public override void Exit()
            {
                base.Exit();

                _waitActor = null;
                _target = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                _eventBus.TriggerActorUnselect(_actor);

                if (_waitActor == null)
                    return;

                if (newSelectable is Actor actor)
                    _target = actor;
                else if(newSelectable is Hexagon hex)
                    _target = hex.Owner;
                else
                    _target = null;

                _waitActor.Activate();
            }

            protected IEnumerator SelectActor_Coroutine(Action<bool> callback)
            {
                Hexagon currentHex = _actor._currentHex;
                List<Hexagon> targets = new(HEX_COUNT_SIDES);

                foreach (var hex in currentHex.Neighbors)
                    if (hex.TrySetSelectableActor(_actor._owner, _targetActor))
                        targets.Add(hex);

                if (targets.Count == 0)
                    yield break;

                yield return _waitActor = new();

                foreach (var hex in targets)
                    hex.SetUnselectable();

                if (_target == null || _target == _actor)
                    yield break;

                Key rKey = _target._currentHex.Key - currentHex.Key;

                if(!ACTOR_ROTATIONS.TryGetValue(rKey, out Quaternion rotation))
                    yield break;

                _parentTransform.localRotation = rotation;

                callback(true);
            }
        }
    }
}
