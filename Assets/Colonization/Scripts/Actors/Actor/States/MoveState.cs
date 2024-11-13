using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public class MoveState : AState
        {
            private readonly float _speed = 0.5f;
            private readonly Transform _parentTransform;
            private WaitActivate _waitHexagon;
            private Hexagon _targetHex;
            private Coroutine _coroutineAction;
            private Ability<ActorAbilityId> _move;

            public MoveState(float speed, Actor parent) : base(parent, 0)
            {
                _speed = speed;
                _parentTransform = _parent._thisTransform;
                _move = _parent._isMove;
            }

            public override void Enter()
            {
                _coroutineAction = _parent.StartCoroutine(SelectHexagon_Coroutine());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _parent.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }

                _waitHexagon = null;
                _targetHex = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                _eventBus.TriggerActorUnselect(_parent);

                if (_waitHexagon == null)
                    return;

                _targetHex = newSelectable as Hexagon;
                _waitHexagon.Activate();
            }

            private void Reset()
            {
                _coroutineAction = null;
                _fsm.Default();
            }

            private IEnumerator Move_Coroutine()
            {
                if (_targetHex == null)
                {
                    Reset();
                    yield break;
                }

                Hexagon currentHex = _parent._currentHex;

                _parentTransform.localRotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
                Vector3 start = currentHex.Position, end = _targetHex.Position;

                currentHex.ExitActor();
                _parent._currentHex = currentHex = _targetHex;
                currentHex.EnterActor(_parent);

                _skin.Move();

                float _progress = 0f;
                while (_progress <= 1f)
                {
                    yield return null;
                    _progress += _speed * Time.deltaTime;
                    _parentTransform.localPosition = Vector3.Lerp(start, end, _progress);
                }

                _parentTransform.localPosition = end;
                _move.IsBaseValue = false;

                Reset();
            }

            private IEnumerator SelectHexagon_Coroutine()
            {
                Hexagon currentHex = _parent._currentHex;

                List<Hexagon> empty = new(6);
                foreach (var hex in currentHex.Neighbors)
                    if (hex.CanUnitEnter)
                        empty.Add(hex);

                if (empty.Count == 0)
                {
                    Reset();
                    yield break;
                }

                //if (empty.Count == 1)
                //{
                //    _targetHex = empty[0];
                //    _coroutineMove = _parent.StartCoroutine(Move_Coroutine());
                //    yield break;
                //}

                _waitHexagon = new();

                foreach (var hex in empty)
                    hex.TrySetSelectable();

                yield return _waitHexagon;

                foreach (var hex in empty)
                    hex.SetUnselectable();

                _coroutineAction = _parent.StartCoroutine(Move_Coroutine());
            }
        }
    }
}
