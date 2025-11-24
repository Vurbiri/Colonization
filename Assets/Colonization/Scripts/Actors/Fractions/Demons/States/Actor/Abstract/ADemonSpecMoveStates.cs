using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Demon
    {
        public abstract class ADemonSpecMoveStates : ADemonStates<DemonSpecMoveSkin>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected ADemonSpecMoveStates(Demon actor, ActorSettings settings) : base(actor, settings) { }

            #region ASpecMoveState
            //******************************************************************************
            protected abstract class ASpecMoveState : AActionState
            {
                private readonly ScaledMoveUsingLerp _move;
                private readonly float _speed;
                private readonly HitEffects _effectsHint;
                protected Coroutine _coroutine;
                protected Hexagon _target;
                protected Key _direction;

                public ASpecMoveState(SpecSkillSettings specSkill, float speed, ADemonSpecMoveStates parent) : base(parent, CONST.SPEC_SKILL_ID, specSkill.Cost)
                {
                    _speed = speed * 1.5f;
                    _move = new(Actor._thisTransform, 0f);
                    _effectsHint = specSkill.HitEffects[0];
                }

                sealed public override void Enter()
                {
                    if (_target != null)
                        _coroutine = StartCoroutine(Move_Cn());
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

                    _target = null;

                    _move.Skip();
                    signal.Send();
                }

                protected IEnumerator Move_Cn()
                {
                    yield return GameContainer.CameraController.ToPositionControlled(CurrentHex.Position);

                    var currentHex = CurrentHex;
                    CurrentHex.ActorExit();
 
                   _effectsHint.Apply(Actor, Actor);
                    Pay();
                    yield return Skin.SpecMove();

                    Rotation = HEX.ROTATIONS[_direction];
                    GameContainer.CameraController.ToPositionControlled(_target.Position);
                    yield return _move.Run(currentHex.Position, _target.Position, _speed / HEX.Distance(currentHex.Key, _target.Key));

                    CurrentHex = _target;
                    CurrentHex.ActorEnter(Actor);

                    _coroutine = null;
                    GetOutOfThisState();
                }
            }
            //******************************************************************************
            #endregion
        }
    }
}
