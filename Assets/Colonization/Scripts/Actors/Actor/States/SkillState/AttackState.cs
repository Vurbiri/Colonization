//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\AttackState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public class AttackState : ASkillTargetState
        {
            private readonly float _speedRun;
            private readonly float _selfRange;

            public AttackState(Actor parent, int targetActor, IReadOnlyList<AEffect> effects, float range, float speedRun, Settings settings, int id) : 
                base(parent, targetActor, effects, settings, id)
            {
                _speedRun = speedRun;
                _selfRange = range + _actor._extentsZ;
            }

            protected override IEnumerator Actions_Coroutine()
            {
                bool isTarget = false;
                yield return _actor.StartCoroutine(SelectActor_Coroutine(b => isTarget = b));
                if (!isTarget)
                {
                    Reset();
                    yield break;
                }

                float path = 1f - (_selfRange + _actor._extentsZ) / HEX_DIAMETER_IN;
                Hexagon currentHex = _actor._currentHex;
                yield return _actor.StartCoroutine(Move_Coroutine(currentHex.Position, _targetHex.Position, path));
                yield return _actor.StartCoroutine(ApplySkill_Coroutine());
                yield return _actor.StartCoroutine(Move_Coroutine(_parentTransform.localPosition, currentHex.Position, 1f));
                
                Reset();
            }

            private IEnumerator Move_Coroutine(Vector3 start, Vector3 end, float path)
            {
                yield return null;

                _skin.Run();

                float progress = 0f;
                while (progress <= path)
                {
                    yield return null;
                    progress += _speedRun * Time.deltaTime;
                    _parentTransform.localPosition = Vector3.Lerp(start, end, progress);
                }
            }


        }
    }
}
