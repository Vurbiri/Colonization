using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public abstract partial class Actor
    {
        public abstract partial class AStates<TActor, TSkin>
        {
            protected class MeleeSkillState : ATargetSkillState
            {
                protected readonly float _speedRun;
                protected readonly float _rangeSkill;

                public MeleeSkillState(AStates<TActor, TSkin> parent, SkillSettings skill, float speedRun, int id) : base(parent, skill, id)
                {
                    _speedRun = speedRun;
                    _rangeSkill = skill.Range;
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
                    float path = 1f - (_rangeSkill + TargetOffset) / HEX.DIAMETER_IN;

                    yield return Run_Cn(actorHexPos, targetHexPos, path);
                    yield return ApplySkill_Cn();
                    yield return Run_Cn(Position, actorHexPos, 1f);

                    ToExit();
                }

                protected IEnumerator Run_Cn(Vector3 start, Vector3 end, float path)
                {
                    yield return null;
                    Skin.Run();
                    yield return Movement_Cn(start, end, _speedRun, path);
                }

                protected IEnumerator Movement_Cn(Vector3 start, Vector3 end, float speed, float path)
                {
                    Vector3 delta = end - start;
                    float progress = 0f;
                    while (progress < path)
                    {
                        progress += Time.deltaTime * speed;
                        Position = new(start.x + delta.x * progress, start.y + delta.y * progress, start.z + delta.z * progress);
                        yield return null;
                    }
                    Position = new(start.x + delta.x * path, start.y + delta.y * path, start.z + delta.z * path);
                }
            }
        }
    }
}
