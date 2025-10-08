using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            sealed protected class MovementSkillState : SkillState
            {
                private readonly float _distanceMove;
                private readonly float _timeToHit;

                public MovementSkillState(AStates<TActor, TSkin> parent, SkillSettings skill, float speedRun, int id) : base(parent, skill, speedRun, id)
                {
                    _distanceMove = skill.Distance;
                    _timeToHit = parent._skin.GetFirsHitTime(id);
                }

                protected override IEnumerator Actions_Cn()
                {
                    yield return SelectActor_Cn();

                    if (_target == null)
                    {
                        ToExit();
                        yield break;
                    }

                    Vector3 actorHexPos = CurrentHex.Position, targetHexPos = TargetHex.Position;
                    float distance = _distanceMove + TargetOffset;

                    if (distance > HEX.DIAMETER_IN)
                        distance = HEX.DIAMETER_IN;
                    else
                        yield return Run_Cn(actorHexPos, targetHexPos, 1f - distance / HEX.DIAMETER_IN);

                    yield return ApplyMovementSkill_Cn(Position, targetHexPos, distance);
                    yield return Run_Cn(Position, actorHexPos, 1f);

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
}
