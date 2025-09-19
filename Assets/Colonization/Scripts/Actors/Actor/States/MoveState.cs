using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class MoveState : AState
            {
                private readonly ScaledMoveUsingLerp _move;
                private Coroutine _coroutine;
                private WaitSignal _waitHexagon;
                private Hexagon _targetHex;

                public readonly WaitSignal signal = new();

                public MoveState(float speed, AStates<TActor, TSkin> parent) : base(parent) => _move = new(Actor._thisTransform, speed);

                public override void Enter()
                {
                    if (_isPerson)
                        _coroutine = StartCoroutine(PersonSelectHexagon_Cn());
                    else
                        _coroutine = StartCoroutine(AISelectHexagon_Cn());
                }

                public override void Exit()
                {
                    if (_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                        _coroutine = null;
                    }

                    _move.Skip();

                    _waitHexagon = null;
                    _targetHex = null;

                    signal.Send();
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private void ToExit()
                {
                    _coroutine = null;
                    GetOutOfThisState();
                }

                public override void Unselect(ISelectable newSelectable)
                {
                    if (_waitHexagon == null)
                        return;

                    _targetHex = newSelectable as Hexagon;
                    _waitHexagon.Send();
                }
                
                private IEnumerator PersonSelectHexagon_Cn()
                {
                    List<Hexagon> empty = new(HEX.SIDES);
                    foreach (var hex in CurrentHex.Neighbors)
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

                    for (int i = empty.Count - 1; i >= 0; i--)
                        empty[i].SetUnselectable();

                    Move();
                }

                private IEnumerator AISelectHexagon_Cn()
                {
                    yield return _waitHexagon = new();

                    Move();
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                private void Move()
                {
                    if (_targetHex == null)
                        ToExit();
                    else
                        _coroutine = StartCoroutine(Move_Cn());
                }

                private IEnumerator Move_Cn()
                {
                    var currentHex = CurrentHex;

                    CurrentHex.ActorExit();
                    CurrentHex = _targetHex;

                    Moving.Off(); Skin.Move();

                    Rotation = CONST.ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
                    yield return _move.Run(currentHex.Position, _targetHex.Position);

                    CurrentHex.ActorEnter(Actor);

                    ToExit();
                }
            }
        }
    }
}
