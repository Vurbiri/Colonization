using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class MoveState : AActionState
            {
                private readonly ScaledMoveUsingLerp _move;
                
                private WaitSignal _waitHexagon;
                private Hexagon _targetHex;
                private Coroutine _coroutine;

                public readonly WaitSignal signal = new();

                public MoveState(float speed, AStates<TActor, TSkin> parent) : base(parent)
                {
                    _move = new(Actor._thisTransform, speed);
                }

                public override void Enter()
                {
                    signal.Reset();

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

                public override void Unselect(ISelectable newSelectable)
                {
                    if (_waitHexagon == null)
                        return;

                    _targetHex = newSelectable as Hexagon;
                    _waitHexagon.Send();
                }

                private void ToExit()
                {
                    _coroutine = null;
                    GetOutOfThisState();
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

                    if (_targetHex == null)
                    {
                        ToExit();
                        yield break;
                    }

                    _coroutine = StartCoroutine(Move_Cn());
                }

                private IEnumerator AISelectHexagon_Cn()
                {
                    yield return _waitHexagon = new();

                    if (_targetHex == null)
                    {
                        ToExit();
                        yield break;
                    }

                    _coroutine = StartCoroutine(Move_Cn());
                }

                private IEnumerator Move_Cn()
                {
                    var currentHex = CurrentHex;

                    CurrentHex.ExitActor();
                    CurrentHex = _targetHex;

                    Moving.Off(); Skin.Move();

                    Rotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
                    yield return _move.Run(currentHex.Position, _targetHex.Position);

                    CurrentHex.EnterActor(Actor);

                    ToExit();
                }
            }
        }
    }
}
