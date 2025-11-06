using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class MoveState : AActionState
            {
                private readonly ScaledMoveUsingLerp _move;
                private readonly WaitSignal _waitHexagon = new();
                private Coroutine _coroutine;
                private Hexagon _targetHex;

                public override bool CanUse { [Impl(256)] get => base.CanUse & Actor._move.IsTrue; }

                public MoveState(float speed, AStates<TActor, TSkin> parent) : base(parent, CONST.MOVE_SKILL_ID, CONST.MOVE_SKILL_COST) => _move = new(Actor._thisTransform, speed);

                public override void Enter()
                {
                    if (IsPerson)
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
                    _waitHexagon.Cancel();
                    _targetHex = null;

                    signal.Send();
                }

                [Impl(256)]
                private void ToExit()
                {
                    _coroutine = null;
                    GetOutOfThisState();
                }

                public override void Unselect(ISelectable newSelectable)
                {
                    if (_waitHexagon.IsWait)
                    {
                        _targetHex = newSelectable as Hexagon;
                        _waitHexagon.Send();
                    }
                }

                private IEnumerator PersonSelectHexagon_Cn()
                {
                    List<Hexagon> empty = new(HEX.SIDES);
                    var neighbors = CurrentHex.Neighbors;
                    for (int i = 0; i < neighbors.Count; i++)
                        if (neighbors[i].TrySetSelectableFree())
                            empty.Add(neighbors[i]);

                    if (empty.Count == 0)
                    {
                        ToExit();
                        yield break;
                    }

                    IsCancel.True();
                    yield return _waitHexagon.Restart();
                    IsCancel.False();

                    for (int i = empty.Count - 1; i >= 0; i--)
                        empty[i].SetUnselectable();

                    Move();
                }

                private IEnumerator AISelectHexagon_Cn()
                {
                    yield return _waitHexagon.Restart();

                    Move();
                }

                [Impl(256)]
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

                    Pay();
                    Skin.Move();

                    Rotation = HEX.ROTATIONS[_targetHex.Key - currentHex.Key];
                    yield return _move.Run(currentHex.Position, _targetHex.Position);

                    CurrentHex.ActorEnter(Actor);

                    ToExit();
                }
            }
        }
    }
}
