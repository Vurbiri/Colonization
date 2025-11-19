using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public static class ActorExt
	{
        private static readonly WaitFrames s_waitBeforeSelecting = new(10);

        [Impl(256)] public static Coroutine UseSkill_Cn(this Actor user, Actor target, int skillId) => user.StartCoroutine(UseSkillInternal_Cn(user, target, skillId));
        [Impl(256)] public static Coroutine Move_Cn(this Actor user, Hexagon target) => user.StartCoroutine(MoveInternal_Cn(user, target));

        private static IEnumerator UseSkillInternal_Cn(Actor user, Actor target, int skillId)
        {
            yield return GameContainer.CameraController.ToPositionControlled(target);

            var wait = user.Action.UseSkill(skillId);

            yield return s_waitBeforeSelecting;
            user.Unselect(target);

            yield return wait;
        }

        private static IEnumerator MoveInternal_Cn(Actor user, Hexagon target)
        {
            yield return GameContainer.CameraController.ToPositionControlled(target.Position);

            var wait = user.Action.UseMoveSkill();

            yield return s_waitBeforeSelecting;
            user.Unselect(target);

            yield return wait;
        }
    }
}
