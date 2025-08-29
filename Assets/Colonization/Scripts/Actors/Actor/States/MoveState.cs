using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        sealed protected class MoveState : AActionState<ActorSkin>
        {
            private readonly ScaledMoveUsingLerp _move;
            private readonly WaitSignal _waitSignal = new();
            private WaitSignal _waitHexagon;
            private Hexagon _targetHex;
            private Coroutine _coroutineAction;

            public MoveState(float speed, Actor parent) : base(parent, parent._skin)
            {
                _move = new(parent._thisTransform, speed);
            }

            public WaitSignal Signal => _waitSignal;

            public override void Enter()
            {
                _waitSignal.Reset();

                if (_isPlayer)
                    _coroutineAction = StartCoroutine(SelectHexagon_Cn());
                else
                    _coroutineAction = StartCoroutine(SelectHexagonAI_Cn());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    StopCoroutine(_coroutineAction);
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
                List<Hexagon> empty = new(HEX.SIDES);
                foreach (var hex in ActorHex.Neighbors)
                    if (hex.TrySetSelectableFree())
                        empty.Add(hex);

                if (empty.Count == 0)
                {
                    ToExit();
                    yield break;
                }

                IsCancel.True();
                yield return _waitHexagon = new();
                IsCancel.False();

                foreach (var hex in empty)
                    hex.SetUnselectable();

                if (_targetHex == null)
                {
                    ToExit(); 
                    yield break;
                }

                _coroutineAction = StartCoroutine(Move_Cn());
            }

            private IEnumerator SelectHexagonAI_Cn()
            {
                yield return _waitHexagon = new();

                if (_targetHex == null)
                {
                    ToExit();
                    yield break;
                }

                _coroutineAction = StartCoroutine(Move_Cn());
            }

            private IEnumerator Move_Cn()
            {
                var currentHex = ActorHex;

                ActorHex.ExitActor();
                ActorHex = _targetHex;

                Moving.Off(); _skin.Move();

                ActorRotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
                yield return _move.Run(currentHex.Position, _targetHex.Position);

                ActorHex.EnterActor(_actor);

                ToExit();
            }
        }
    }
}
