using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.Actors
{
    public partial class Demon
    {
        public abstract partial class ADemonSpecMoveStates
        {
            protected abstract class ASpecMoveState : AActionState
            {
                private readonly ScaledMoveUsingLerp _move;
                private readonly HitEffects _effectsHint;
                protected Coroutine _coroutine;

                public readonly WaitSignal signal = new();

                public ASpecMoveState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(parent, specSkill.Cost)
                {
                    _move = new(Actor._thisTransform, speed);
                    _effectsHint = specSkill.HitEffects[0];
                }

                sealed public override void Enter()
                {
                    if (TryGetTarget(out Hexagon targetHex, out Key direction))
                        _coroutine = StartCoroutine(Move_Cn(targetHex, direction));
                    else
                        GetOutOfThisState();
                }

                sealed public override void Exit()
                {
                    if (_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                        _coroutine = null;
                    }

                    _move.Skip();
                    signal.Send();
                }

                protected IEnumerator Move_Cn(Hexagon targetHex, Key direction)
                {
                    var currentHex = CurrentHex;

                    CurrentHex.ExitActor();
                    CurrentHex = targetHex;

                    Pay();
                    _effectsHint.Apply(Actor, Actor);
                    yield return Skin.SpecMove();

                    Rotation = CONST.ACTOR_ROTATIONS[direction];
                    yield return _move.Run(currentHex.Position, targetHex.Position);

                    CurrentHex.EnterActor(Actor);

                    _coroutine = null;
                    GetOutOfThisState();
                }

                protected abstract bool TryGetTarget(out Hexagon targetHex, out Key direction);
            }
        }
    }
}
