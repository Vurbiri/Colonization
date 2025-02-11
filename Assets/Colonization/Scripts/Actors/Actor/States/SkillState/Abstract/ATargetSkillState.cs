//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\Abstract\ASkillTargetState.cs
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
        public abstract class ATargetSkillState : ASkillState
        {
            protected Actor _target;
            protected bool _isTargetReact;
            protected WaitActivate _waitActor;
            protected readonly ReactiveValue<bool> _isCancel;
            protected readonly WaitForSecondsRealtime _waitRealtime = new(0.6f);
            protected readonly Relation _relationTarget;

            protected ATargetSkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<EffectsHit> effects, bool isTargetReact, int cost, int id) : 
                base(parent, effects, cost, id)
            {
                _isCancel = parent._canCancel;
                _isTargetReact = isTargetReact;
                _relationTarget = targetActor.ToRelation();
                Debug.Log("������� _relationTarget = Relation.Friend;");
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

                _isCancel.Value = true;
                yield return _waitActor = new();
                _isCancel.Value = false;

                foreach (var hex in targets)
                    hex.SetOwnerUnselectable();

                if (_target == null)
                    yield break;

                Pay();
                Hexagon targetHex = _target._currentHex;
                _parentTransform.localRotation = ACTOR_ROTATIONS[targetHex.Key - currentHex.Key];
                if(_isTargetReact)
                    _target._thisTransform.localRotation = ACTOR_ROTATIONS[currentHex.Key - targetHex.Key];

                callback(true);
            }

            protected override IEnumerator ApplySkill_Coroutine()
            {
                CustomYieldInstruction wait = _skin.Skill(_id, _target._thisTransform);

                for (int i = 0; i < _countHits; i++)
                {
                    yield return wait;
                    _effectsHint[i].Apply(_actor, _target);
                    if (_target.Hit(_isTargetReact))
                    {
                        wait = _waitRealtime;
                        break;
                    }
                    wait.Reset();
                }
                yield return wait;
                _target.SkillUsedEnd();
            }

            private Actor CheckTarget(Actor target)
            {
                if (target == null)
                    return null;

                Key key = target._currentHex.Key - _actor._currentHex.Key;

                if (target == _actor || !target.IsCanUseSkill(_actor._owner, _relationTarget, out _) || !ACTOR_ROTATIONS.ContainsKey(key))
                    return null;

                target.SkillUsedStart(_actor._owner, _relationTarget);
                return target;
            }
        }
    }
}
