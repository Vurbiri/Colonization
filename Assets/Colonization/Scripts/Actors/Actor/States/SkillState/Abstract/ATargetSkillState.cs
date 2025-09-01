using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected abstract class ATargetSkillState : ASkillState
            {
                protected Actor _target;
                private WaitSignal _waitActor;
                private readonly WaitRealtime _waitRealtime = new(0.3f);
                private readonly Relation _relationTarget;
                // !!!!!!!!!!!!!!!!!!!!! удалить _relationRealTarget
                private readonly Relation _relationRealTarget;

                #region Propirties
                protected Hexagon TargetHex { [Impl(256)] get => _target._currentHex; }
                protected ActorSkin TargetSkin { [Impl(256)] get => _target._states.Skin; }

                protected Key KeyActor { [Impl(256)] get => _parent._actor._currentHex.Key; }
                protected Key KeyTarget { [Impl(256)] get => _target._currentHex.Key; }

                protected float TargetOffset { [Impl(256)] get => _target._extentsZ; }

                protected Id<PlayerId> Owner { [Impl(256)] get => _parent._actor._owner; }
                #endregion

                protected ATargetSkillState(AStates<TActor, TSkin> parent, SkillSettings skill, int id) : base(parent, skill, id)
                {
                    _relationTarget = skill.Target.ToRelation();
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

                    if (_target != null)
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

                [Impl(256)]
                protected IEnumerator SelectActor_Cn()
                {
                    _target = null;
                    if (_isPerson)
                        yield return PersonSelectActor_Cn();
                    else
                        yield return AISelectActor_Cn();
                }

                sealed protected override IEnumerator ApplySkill_Cn()
                {
                    IEnumerator wait = Skin.Skill(_id, _target._states.Skin);

                    for (int i = 0; i < _countHits; i++)
                    {
                        yield return wait;
                        _effectsHint[i].Apply(Actor, _target);

                        if (_target.IsDead)
                        {
                            GameContainer.TriggerBus.TriggerActorKill(Owner, _target._typeId, _target._id);
                            wait = _waitRealtime;
                            break;
                        }

                        wait.Reset();
                    }

                    yield return wait;
                }

                private IEnumerator PersonSelectActor_Cn()
                {
                    List<Hexagon> targets = new(HEX.SIDES);
                    foreach (var hex in CurrentHex.Neighbors)
                        if (hex.TrySetOwnerSelectable(Owner, _relationTarget))
                            targets.Add(hex);

                    if (targets.Count == 0)
                        yield break;

                    IsCancel.True();
                    yield return _waitActor = new();
                    IsCancel.False();

                    for (int i = targets.Count - 1; i >= 0; i--)
                        targets[i].SetOwnerUnselectable();



                    if (_target == null)
                        yield break;

                    Pay();
                    RotateActors();
                }

                private IEnumerator AISelectActor_Cn()
                {
                    yield return _waitActor = new();

                    if (_target == null)
                        yield break;

                    Pay();
                    RotateActors();
                }

                [Impl(256)]
                private void RotateActors()
                {
                    Rotation = ACTOR_ROTATIONS[KeyTarget - KeyActor];
                    if (_relationRealTarget == Relation.Enemy)
                        _target._thisTransform.localRotation = ACTOR_ROTATIONS[KeyActor - KeyTarget];
                }
            }
        }
    }
}
