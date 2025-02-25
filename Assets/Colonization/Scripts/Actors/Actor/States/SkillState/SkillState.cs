//Assets\Colonization\Scripts\Actors\Actor\States\SkillState\SkillState.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

            public SkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<EffectsHit> effects, float range, float speedRun, int cost, int id) : 
                base(parent, targetActor, effects, cost, id)
            {
                _speedRun = speedRun;
                _rangeSkill = range;
            }

            public override void Exit()
            {
                base.Exit();

                _parentTransform.localPosition = _actor._currentHex.Position;
            }

            protected override IEnumerator Actions_Cn()
            {
                bool isTarget = false;
                yield return SelectActor_Cn(b => isTarget = b);
                if (!isTarget) 
                { 
                    ToExit(); 
                    yield break;
                }

                Hexagon currentHex = _actor._currentHex, targetHex = _target._currentHex;
                float path = 1f - (_rangeSkill + _target._extentsZ) / HEX_DIAMETER_IN;

                yield return Run_Cn(currentHex.Position, targetHex.Position, path);
                yield return ApplySkill_Cn();
                yield return Run_Cn(_parentTransform.localPosition, currentHex.Position, 1f);
                
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
                float progress = 0f;
                while (progress <= path)
                {
                    yield return null;
                    progress += speed * Time.deltaTime;
                    _parentTransform.localPosition = Vector3.Lerp(start, end, progress);
                }
            }
        }
    }
}
