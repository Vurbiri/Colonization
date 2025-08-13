using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Collections;
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
            protected readonly WaitRealtime _waitRealtime = new(0.3f);
            protected readonly Relation _relationTarget;
            // !!!!!!!!!!!!!!!!!!!!! удалить _relationRealTarget
            protected readonly Relation _relationRealTarget;

            protected ATargetSkillState(Actor parent, TargetOfSkill targetActor, ReadOnlyArray<HitEffects> effects, int cost, int id) : 
                base(parent, effects, cost, id)
            {
                _isCancel = parent._canCancel;
                _relationTarget = targetActor.ToRelation();
                Debug.Log("удалить _relationTarget = Relation.Friend; и _relationRealTarget");
                _relationRealTarget = _relationTarget;
                _relationTarget = Relation.Friend;
            }

            sealed public override void Enter()
            {
                _waitActor = null;
                _target = null;

                base.Enter();
            }

            sealed public override void Exit()
            {
                base.Exit();

                if(_target != null)
                    _target.FromTargetState();
            }

            sealed public override void Unselect(ISelectable newSelectable)
            {
                if (_waitActor == null)
                    return;

                _target = CheckTarget(newSelectable as Actor);
                _waitActor.Send();

                #region Local: CheckTarget(..)
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                Actor CheckTarget(Actor target)
                {
                    if (target != null)
                    {
                        if ((target._currentHex.Key ^ _actor._currentHex.Key) == 1 & target.IsCanApplySkill(_actor._owner, _relationTarget, out _))
                            target.ToTargetState(_actor._owner, _relationTarget);
                        else
                            target = null;
                    }
                    return target;
                }
                #endregion
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

            sealed protected override IEnumerator ApplySkill_Cn()
            {
                IEnumerator wait = _skin.Skill(_id, _target._skin);

                for (int i = 0; i < _countHits; i++)
                {
                    yield return wait;
                    _effectsHint[i].Apply(_actor, _target);

                    if (_target.IsDead)
                    {
                        GameContainer.TriggerBus.TriggerActorKill(_actor._owner, _target._typeId, _target._id);
                        wait = _waitRealtime;
                        i = _countHits;
                    }

                    wait.Reset();
                }

                yield return wait;
                _target.FromTargetState(); _target = null;
            }

            private void RotateActors()
            {
                Hexagon currentHex = _actor._currentHex, targetHex = _target._currentHex;
                _parentTransform.localRotation = ACTOR_ROTATIONS[targetHex.Key - currentHex.Key];
                if (_relationRealTarget == Relation.Enemy)
                    _target._thisTransform.localRotation = ACTOR_ROTATIONS[currentHex.Key - targetHex.Key];
            }
        }
    }
}
