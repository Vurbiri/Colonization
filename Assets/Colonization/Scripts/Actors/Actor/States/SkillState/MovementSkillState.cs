using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        sealed protected class MovementSkillState : SkillState
        {
            private readonly float _distanceMove;
            private readonly float _timeToHit;

            public MovementSkillState(Actor parent, TargetOfSkill targetActor, ReadOnlyArray<HitEffects> effects, float distance, float range, float speedRun, int cost, int id) : base(parent, targetActor, effects, range, speedRun, cost, id)
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

                Vector3 actorHexPos = ActorHex.Position, targetHexPos = TargetHex.Position;
                float distance = _distanceMove + TargetOffset;

                if(distance > HEX_DIAMETER_IN) 
                    distance = HEX_DIAMETER_IN;
                else 
                    yield return Run_Cn(actorHexPos, targetHexPos, 1f - distance / HEX_DIAMETER_IN);

                yield return ApplyMovementSkill_Cn(ActorPosition, targetHexPos, distance);
                yield return Run_Cn(ActorPosition, actorHexPos, 1f);

                ToExit();
            }

            private IEnumerator ApplyMovementSkill_Cn(Vector3 start, Vector3 end, float remainingDistance)
            {
                float distance = _rangeSkill + TargetOffset;
                float path = 1f - distance / remainingDistance;
                float speed = path / _timeToHit;
                
                StartCoroutine(Movement_Cn(start, end, speed, path));

                yield return ApplySkill_Cn();
            }
        }
    }
}
