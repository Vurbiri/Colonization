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
        public class SkillState : ATargetSkillState
        {
            private readonly float _speedRun;
            private readonly float _selfRange;

            public SkillState(Actor parent, TargetOfSkill targetActor, IReadOnlyList<EffectsHit> effects, bool isTargetReact, float range, float speedRun, int cost, int id) : 
                base(parent, targetActor, effects, isTargetReact, cost, id)
            {
                
                _speedRun = speedRun;
                _selfRange = range + _actor._extentsZ;
            }

            public override void Exit()
            {
                base.Exit();

                _parentTransform.localPosition = _actor._currentHex.Position;
            }

            protected override IEnumerator Actions_Coroutine()
            {
                bool isTarget = false;
                yield return SelectActor_Coroutine(b => isTarget = b);
                if (!isTarget) 
                { 
                    ToExit(); yield break;
                }

                Hexagon currentHex = _actor._currentHex, targetHex = _target._currentHex;
                float path = 1f - (_selfRange + _actor._extentsZ) / HEX_DIAMETER_IN;

                yield return Move_Coroutine(currentHex.Position, targetHex.Position, path);
                yield return ApplySkill_Coroutine();
                yield return Move_Coroutine(_parentTransform.localPosition, currentHex.Position, 1f);
                
                ToExit();
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
