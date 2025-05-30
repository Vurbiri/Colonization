using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class ATargetSkillState : ASkillState
        {
            protected Actor _target;
            protected WaitSignal _waitActor;
            protected readonly RBool _isCancel;
            protected readonly WaitRealtime _waitRealtime = new(0.6f);
            protected readonly Relation _relationTarget;
            // !!!!!!!!!!!!!!!!!!!!! удалить _relationRealTarget
            protected readonly Relation _relationRealTarget;

            protected ATargetSkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<HitEffects> effects, int cost, int id) : 
                base(parent, effects, cost, id)
            {
                _isCancel = parent._canCancel;
                _relationTarget = targetActor.ToRelation();
                Debug.Log("удалить _relationTarget = Relation.Friend; и _relationRealTarget");
                _relationRealTarget = _relationTarget;
                _relationTarget = Relation.Friend;
            }

            public override void Exit()
            {
                base.Exit();

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

                _waitActor.Send();
            }

            protected IEnumerator SelectActor_Cn(Action<bool> callback)
            {
                Hexagon currentHex = _actor._currentHex;
                List<Hexagon> targets = new(HEX.SIDES);

                foreach (var hex in currentHex.Neighbors)
                    if (hex.TrySetOwnerSelectable(_actor._owner, _relationTarget))
                        targets.Add(hex);

                if (targets.Count == 0)
                    yield break;

                _isCancel.True();
                yield return _waitActor = new();
                _isCancel.False();

                foreach (var hex in targets)
                    hex.SetOwnerUnselectable();

                if (_target == null)
                    yield break;

                Pay();
                RotateActors();

                callback(true);
            }

            protected IEnumerator SelectActorAI_Cn(Action<bool> callback)
            {
                yield return _waitActor = new();

                if (_target == null)
                    yield break;

                Pay();
                RotateActors();

                callback(true);
            }

            protected override IEnumerator ApplySkill_Cn()
            {
                IEnumerator wait = _skin.Skill(_id, _target._skin);

                for (int i = 0; i < _countHits; i++)
                {
                    yield return wait;
                    _effectsHint[i].Apply(_actor, _target);
                    if (_target.IsDead)
                    {
                        _actor._eventKilled.Invoke(_target._owner, _target._id);
                        wait = _waitRealtime;
                        break;
                    }
                    wait.Reset();
                }
                yield return wait;
                _target.FromTargetState();
            }

            private void RotateActors()
            {
                Hexagon currentHex = _actor._currentHex, targetHex = _target._currentHex;
                _parentTransform.localRotation = ACTOR_ROTATIONS[targetHex.Key - currentHex.Key];
                if (_relationRealTarget == Relation.Enemy)
                    _target._thisTransform.localRotation = ACTOR_ROTATIONS[currentHex.Key - targetHex.Key];
            }

            private Actor CheckTarget(Actor target)
            {
                if (target == null)
                    return null;

                Key key = target._currentHex.Key - _actor._currentHex.Key;

                if (target == _actor | key.Distance != 1 || !target.IsCanUseSkill(_actor._owner, _relationTarget, out _))
                    return null;

                target.ToTargetState(_actor._owner, _relationTarget);
                return target;
            }
        }
    }
}
