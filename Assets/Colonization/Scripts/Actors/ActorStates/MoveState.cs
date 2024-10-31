using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public class MoveState : AActorState
    {
        private readonly float _speed = 0.5f;
        private readonly Transform _parentTransform;
        private readonly Action<Hexagon> setCurrentHex;
        private readonly ActorSkin _skin;
        private WaitActivate _waitHexagon;
        private Hexagon _targetHex;
        private Coroutine _coroutineMove;

        public MoveState(float speed, Action<Hexagon> setor, Warrior parent) : base(parent, 0)
        {
            _speed = speed;
            setCurrentHex = setor;
            _parentTransform = _parent.transform;
            _skin = _parent.Skin;
        }

        public override void Enter()
        {
            _parent.StartCoroutine(SelectHexagon_Coroutine());
        }

        public override void Exit()
        {
            if(_coroutineMove != null)
            {
                _parent.StopCoroutine(_coroutineMove);
                _coroutineMove = null;
            }

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

        private void Reset()
        {
            _coroutineMove = null;
            _fsm.Reset();
        }

        private IEnumerator Move_Coroutine()
        {
            if (_targetHex == null)
            {
                Reset();
                yield break;
            }

            Hexagon currentHex = _parent.CurrentHex;

            _parentTransform.localRotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];
            Vector3 start = currentHex.Position, end = _targetHex.Position;

            currentHex.ExitActor();
            setCurrentHex(currentHex = _targetHex);
            currentHex.EnterActor(_parent.Owner);

            _skin.StartMove();

            float _progress = 0f;
            while (_progress <= 1f)
            {
                yield return null;
                _progress += _speed * Time.deltaTime;
                _parentTransform.localPosition = Vector3.Lerp(start, end, _progress);
            }

            _parentTransform.localPosition = end;
            _skin.ResetStates();

            _fsm.SetState<IdleState>();
        }

        private IEnumerator SelectHexagon_Coroutine()
        {
            Hexagon currentHex = _parent.CurrentHex;

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

            foreach (var hex in currentHex.Neighbors)
                hex.SetUnselectable();

            _coroutineMove = _parent.StartCoroutine(Move_Coroutine());
        }
    }
}
