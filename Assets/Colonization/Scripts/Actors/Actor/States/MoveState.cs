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
            private readonly ScaledMoveUsingLerp _move;
            private readonly Transform _parentTransform;
            private readonly RBool _isCancel;
            private WaitSignal _waitHexagon;
            private Hexagon _targetHex;
            private Coroutine _coroutineAction;
            private readonly WaitSignal _waitSignal = new();

            public MoveState(float speed, Actor parent) : base(parent)
            {
                _parentTransform = parent._thisTransform;
                _move = new(_parentTransform, speed);
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

                _move.Skip();

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
                GetOutOfThisState();
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
                var currentHex = _actor._currentHex;
                currentHex.ExitActor();
                
                _actor._currentHex = _targetHex;
                _moveAbility.Off();
                 _skin.Move();

                _parentTransform.localRotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
                yield return _move.Run(currentHex.Position, _targetHex.Position);

                _targetHex.EnterActor(_actor);

                ToExit();
            }
        }
    }
}
