//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\Abstract\ASkillTargetState.cs
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract class ASkillTargetState : ASkillState
        {
            protected int _targetActor;
            protected WaitActivate _waitActor;
            protected Hexagon _targetHex;

            protected ASkillTargetState(Actor parent, int targetActor, IReadOnlyList<AEffect> effects, Settings settings, int id) : 
                base(parent, effects, settings, id)
            {
                _targetActor = targetActor;
            }

            public override void Exit()
            {
                base.Exit();

                _waitActor = null;
                _targetHex = null;
            }

            public override void Unselect(ISelectable newSelectable)
            {
                _eventBus.TriggerActorUnselect(_actor);

                if (_waitActor == null)
                    return;

                _targetHex = newSelectable as Hexagon;
                _waitActor.Activate();
            }

            protected IEnumerator SelectActor_Coroutine(Action<bool> callback)
            {
                Hexagon currentHex = _actor._currentHex;
                List<Hexagon> empty = new(6);

                foreach (var hex in currentHex.Neighbors)
                    if (hex.CanUnitEnter)
                        empty.Add(hex);

                if (empty.Count == 0)
                    yield break;


                _waitActor = new();

                foreach (var hex in empty)
                    hex.TrySetSelectable(false);

                yield return _waitActor;

                foreach (var hex in empty)
                    hex.SetUnselectable();

                if (_targetHex == null)
                    yield break;

                _parentTransform.localRotation = ACTOR_ROTATIONS[_targetHex.Key - currentHex.Key];

                callback(true);
            }
        }
    }
}
