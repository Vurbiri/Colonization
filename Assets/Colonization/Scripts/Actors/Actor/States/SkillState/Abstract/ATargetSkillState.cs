using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected abstract class ATargetSkillState : ASkillState
        {
            private Actor _target;
            private WaitSignal _waitActor;
            private readonly WaitRealtime _waitRealtime = new(0.3f);
            private readonly Relation _relationTarget;
            // !!!!!!!!!!!!!!!!!!!!! удалить _relationRealTarget
            private readonly Relation _relationRealTarget;

            #region Propirties
            protected Hexagon TargetHex { [Impl(256)] get => _target._currentHex; }

            protected Key KeyActor { [Impl(256)] get => _actor._currentHex.Key; }
            protected Key KeyTarget { [Impl(256)] get => _target._currentHex.Key; }

            protected float TargetOffset { [Impl(256)] get => _target._extentsZ; }

            protected Id<PlayerId> Owner { [Impl(256)] get => _actor._owner; }
            #endregion

            protected ATargetSkillState(Actor parent, TargetOfSkill targetActor, ReadOnlyArray<HitEffects> effects, int cost, int id) : 
                base(parent, effects, cost, id)
            {

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
                if (_waitActor != null)
                {
                    _target = newSelectable as Actor;
                    if (_target != null && ((KeyTarget ^ KeyActor) != 1 || !_target.ToTargetState(Owner, _relationTarget)))
                        _target = null;

                    _waitActor.Send();
                }
            }

            protected IEnumerator SelectActor_Cn(Action<bool> callback)
            {
                List<Hexagon> targets = new(HEX.SIDES);

                foreach (var hex in ActorHex.Neighbors)
                    if (hex.TrySetOwnerSelectable(Owner, _relationTarget))
                        targets.Add(hex);

                if (targets.Count == 0)
                    yield break;

                IsCancel.True();
                yield return _waitActor = new();
                IsCancel.False();

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
                        GameContainer.TriggerBus.TriggerActorKill(Owner, _target._typeId, _target._id);
                        wait = _waitRealtime;
                        break;
                    }

                    wait.Reset();
                }

                yield return wait;
                //_target.FromTargetState(); _target = null;
            }

            [Impl(256)] private void RotateActors()
            {
                ActorRotation = ACTOR_ROTATIONS[KeyTarget - KeyActor];
                if (_relationRealTarget == Relation.Enemy)
                    _target._thisTransform.localRotation = ACTOR_ROTATIONS[KeyActor - KeyTarget];
            }
        }
    }
}
