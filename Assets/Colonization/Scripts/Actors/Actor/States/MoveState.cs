//Assets\Colonization\Scripts\Actors\Actor\States\MoveState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        protected class MoveState : AActionState
        {
            private readonly float _speed;
            private readonly Transform _parentTransform;
            private readonly ReactiveValue<bool> _isCancel;
            private WaitActivate _waitHexagon;
            private Hexagon _targetHex;
            private Coroutine _coroutineAction;

            public MoveState(float speed, Actor parent) : base(parent)
            {
                _speed = speed;
                _parentTransform = parent._thisTransform;
                _isCancel = parent._canCancel;
            }

            public override void Enter()
            {
                if(_isPlayer)
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
            }

            public override void Unselect(ISelectable newSelectable)
            {
                if (_waitHexagon == null)
                    return;

                _targetHex = newSelectable as Hexagon;
                _waitHexagon.Activate();
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

                _isCancel.Value = true;
                yield return _waitHexagon = new();
                _isCancel.Value = false;

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
