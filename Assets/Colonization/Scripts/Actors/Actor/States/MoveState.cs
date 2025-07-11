using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        sealed protected class MoveState : AActionState
        {
            private readonly float _speed;
            private readonly Transform _parentTransform;
            private readonly RBool _isCancel;
            private WaitSignal _waitHexagon;
            private Hexagon _targetHex;
            private Coroutine _coroutineAction;
            private readonly WaitSignal _waitSignal = new();

            public MoveState(float speed, Actor parent) : base(parent)
            {
                _speed = speed;
                _parentTransform = parent._thisTransform;
                _isCancel = parent._canCancel;
            }

            public WaitSignal Signal => _waitSignal;

            public override void Enter()
            {
                _waitSignal.Reset();

                if (_isPlayer)
                    _coroutineAction = _actor.StartCoroutine(SelectHexagon_Cn());
                else
                    _coroutineAction = _actor.StartCoroutine(SelectHexagonAI_Cn());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _actor.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }

                _parentTransform.localPosition = _actor._currentHex.Position;
                _waitHexagon = null;
                _targetHex = null;

                _waitSignal.Send();
            }

            public override void Unselect(ISelectable newSelectable)
            {
                if (_waitHexagon == null)
                    return;

                _targetHex = newSelectable as Hexagon;
                _waitHexagon.Send();
            }

            private void ToExit()
            {
                _coroutineAction = null;
                _fsm.ToDefaultState();
            }

            private IEnumerator SelectHexagon_Cn()
            {
                Hexagon currentHex = _actor._currentHex;

                List<Hexagon> empty = new(HEX.SIDES);
                foreach (var hex in currentHex.Neighbors)
                    if (hex.TrySetSelectableFree())
                        empty.Add(hex);

                if (empty.Count == 0)
                {
                    ToExit();
                    yield break;
                }

                _isCancel.True();
                yield return _waitHexagon = new();
                _isCancel.False();

                foreach (var hex in empty)
                    hex.SetUnselectable();

                if (_targetHex == null)
                {
                    ToExit(); 
                    yield break;
                }

                _coroutineAction = _actor.StartCoroutine(Move_Cn());
            }

            private IEnumerator SelectHexagonAI_Cn()
            {
                yield return _waitHexagon = new();

                if (_targetHex == null)
                {
                    ToExit();
                    yield break;
                }

                _coroutineAction = _actor.StartCoroutine(Move_Cn());
            }

            private IEnumerator Move_Cn()
            {
                Hexagon currentHex = _actor._currentHex;

                _parentTransform.localRotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
                Vector3 start = currentHex.Position, end = _targetHex.Position;

                currentHex.ExitActor();
                _actor._currentHex = currentHex = _targetHex;
                _move.Off();
                
                _skin.Move();

                float _progress = 0f;
                while (_progress <= 1f)
                {
                    yield return null;
                    _progress += _speed * Time.deltaTime;
                    _parentTransform.localPosition = Vector3.Lerp(start, end, _progress);
                }

                currentHex.EnterActor(_actor);

                ToExit();
            }
        }
    }
}
