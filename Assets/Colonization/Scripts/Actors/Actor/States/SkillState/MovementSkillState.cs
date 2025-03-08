//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\MovementSkillState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected class MovementSkillState : SkillState
        {
            private readonly float _distanceMove;
            private readonly float _timeToHit;

            public MovementSkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<HitEffects> effects, float distance, float range, float speedRun, int cost, int id) : base(parent, targetActor, effects, range, speedRun, cost, id)
            {
                _distanceMove = distance;
                _timeToHit = _skin.GetFirsHitTime(id);
            }

            protected override IEnumerator Actions_Cn()
            {
                bool isTarget = false;
                if (_isPlayer)
                    yield return SelectActor_Cn(b => isTarget = b);
                else
                    yield return SelectActorAI_Cn(b => isTarget = b);

                if (!isTarget)
                {
                    ToExit();
                    yield break;
                }

                Hexagon currentHex = _actor._currentHex, targetHex = _target._currentHex;

                float distance = _distanceMove + _target._extentsZ;
                float path = 1f - distance / HEX_DIAMETER_IN;

                yield return Run_Cn(currentHex.Position, targetHex.Position, path);
                yield return ApplyMovementSkill_Cn(_parentTransform.localPosition, targetHex.Position, distance);
                yield return Run_Cn(_parentTransform.localPosition, currentHex.Position, 1f);

                ToExit();
            }

            private IEnumerator ApplyMovementSkill_Cn(Vector3 start, Vector3 end, float remainingDistance)
            {
                float distance = _rangeSkill + _target._extentsZ;
                float path = 1f - distance / remainingDistance;
                float speed = path / _timeToHit;
                
                _actor.StartCoroutine(Movement_Cn(start, end, speed, path));

                yield return ApplySkill_Cn();
            }
        }
    }
}
