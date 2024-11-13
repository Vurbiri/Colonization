namespace Vurbiri.Colonization.Actors
{
    using System.Collections;
    using UnityEngine;
    using static CONST;

    public abstract partial class Actor
    {
        public class AttackState : ASkillState
        {
            private readonly float _speedRun;
            private readonly float _selfRange;

            public AttackState(Actor parent, int percentDamage, float range, float speedRun, Settings settings, int id) : base(parent, percentDamage, settings, id)
            {
                _speedRun = speedRun;
                _selfRange = range + _parent._extentsZ;
            }

            protected override IEnumerator Actions_Coroutine()
            {
                bool isTarget = false;
                yield return _parent.StartCoroutine(SelectActor_Coroutine(b => isTarget = b));
                if (!isTarget)
                {
                    Reset();
                    yield break;
                }

                float path = 1f - (_selfRange + _parent._extentsZ) / HEX_DIAMETER_IN;
                Hexagon currentHex = _parent._currentHex;
                yield return _parent.StartCoroutine(Move_Coroutine(currentHex.Position, _targetActor.Position, path));
                yield return _parent.StartCoroutine(ApplySkill_Coroutine());
                yield return _parent.StartCoroutine(Move_Coroutine(_parentTransform.localPosition, currentHex.Position, 1f));
                
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
