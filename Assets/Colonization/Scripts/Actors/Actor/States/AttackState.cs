using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    using static CONST;

    public abstract partial class Actor
    {
        public class AttackState : AState
        {
            private readonly Transform _parentTransform;
            private WaitActivate _waitActor;
            private Hexagon _targetActor;
            private Coroutine _coroutineAction;

            public AttackState(Actor parent, int id = 0) : base(parent, id)
            {
                _parentTransform = _parent._thisTransform;
            }

            public override void Enter()
            {
                _coroutineAction = _parent.StartCoroutine(Actions_Coroutine());
            }

            public override void Exit()
            {
                if (_coroutineAction != null)
                {
                    _parent.StopCoroutine(_coroutineAction);
                    _coroutineAction = null;
                }

                _waitActor = null;
                _targetActor = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                _eventBus.TriggerActorUnselect(_parent);

                if (_waitActor == null)
                    return;

                _targetActor = newSelectable as Hexagon;
                _waitActor.Activate();
            }

            private void Reset()
            {
                _coroutineAction = null;
                _fsm.Default();
            }

            private IEnumerator Actions_Coroutine()
            {
                Hexagon currentHex = _parent._currentHex;
                yield return _parent.StartCoroutine(SelectActor_Coroutine(currentHex));
                if (_targetActor == null)
                {
                    Reset();
                    yield break;
                }
                _parentTransform.localRotation = ACTOR_ROTATIONS[_targetActor.Key - currentHex.Key];

                yield return _parent.StartCoroutine(Move_Coroutine(currentHex.Position, _targetActor.Position, 0.5f, _skin.RunForward));
                yield return _parent.StartCoroutine(Attack_Coroutine());
                yield return _parent.StartCoroutine(Move_Coroutine(_parentTransform.localPosition, currentHex.Position, 1f, _skin.RunBack));
                
                Reset();
            }

            private IEnumerator SelectActor_Coroutine(Hexagon currentHex)
            {
                List<Hexagon> empty = new(6);
                foreach (var hex in currentHex.Neighbors)
                    if (hex.CanUnitEnter)
                        empty.Add(hex);

                if (empty.Count == 0)
                    yield break;

                //if (empty.Count == 1)
                //{
                //    _targetHex = empty[0];
                //    _coroutineMove = _parent.StartCoroutine(Move_Coroutine());
                //    yield break;
                //}

                _waitActor = new();

                foreach (var hex in empty)
                    hex.TrySetSelectable(false);

                yield return _waitActor;

                foreach (var hex in empty)
                    hex.SetUnselectable();
            }

            private IEnumerator Move_Coroutine(Vector3 start, Vector3 end, float path, Action animAction)
            {
                animAction();

                float _progress = 0f;
                while (_progress <= path)
                {
                    yield return null;
                    _progress += 0.6f * Time.deltaTime;
                    _parentTransform.localPosition = Vector3.Lerp(start, end, _progress);
                }
            }

            private IEnumerator Attack_Coroutine()
            {
                _skin.Attack();
                yield return new WaitForSeconds(2.12f);
            }

            #region Nested: Profile
            //*******************************************************
            [System.Serializable]
            public struct Attack
            {
                public float time;
                public float percentDamage;
            }
            #endregion
        }
    }
}
