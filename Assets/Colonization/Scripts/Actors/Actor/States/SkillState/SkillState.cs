using System.Collections;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using static Vurbiri.Colonization.CONST;

namespace Vurbiri.Colonization.Actors
{
    public abstract partial class Actor
    {
        protected class SkillState : ATargetSkillState
        {
            protected readonly float _speedRun;
            protected readonly float _rangeSkill;

            public SkillState(Actor parent, TargetOfSkill targetActor, ReadOnlyArray<HitEffects> effects, float range, float speedRun, int cost, int id) : 
                base(parent, targetActor, effects, cost, id)
            {
                _speedRun = speedRun;
                _rangeSkill = range;
            }

            protected override IEnumerator Actions_Cn()
            {
                bool isTarget = false;
                if(_isPlayer)
                    yield return SelectActor_Cn(b => isTarget = b);
                else
                    yield return SelectActorAI_Cn(b => isTarget = b);
                
                if (!isTarget) 
                { 
                    ToExit(); 
                    yield break;
                }

                Vector3 actorHexPos = ActorHex.Position, targetHexPos = TargetHex.Position;
                float path = 1f - (_rangeSkill + TargetOffset) / HEX_DIAMETER_IN;

                yield return Run_Cn(actorHexPos, targetHexPos, path);
                yield return ApplySkill_Cn();
                yield return Run_Cn(ActorPosition, actorHexPos, 1f);
                
                ToExit();
            }

            protected IEnumerator Run_Cn(Vector3 start, Vector3 end, float path)
            {
                yield return null;
                _skin.Run();
                yield return Movement_Cn(start, end, _speedRun, path);
            }

            protected IEnumerator Movement_Cn(Vector3 start, Vector3 end, float speed, float path)
            {
                Vector3 delta = end - start;
                float progress = 0f;
                while (progress < path)
                {
                    progress += Time.deltaTime * speed;
                    ActorPosition = new(start.x + delta.x * progress, start.y + delta.y * progress, start.z + delta.z * progress);
                    yield return null;
                }
                ActorPosition = new(start.x + delta.x * path, start.y + delta.y * path, start.z + delta.z * path);
            }
        }
    }
}
