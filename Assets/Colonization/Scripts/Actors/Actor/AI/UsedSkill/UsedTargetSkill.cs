using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	public abstract class UsedTargetSkill
	{
        [SerializeField] protected int _skill;

        public IEnumerator Use_Cn(Actor user, Actor target, IEnumerator waitBeforeSelecting)
        {
            yield return GameContainer.CameraController.ToPositionControlled(target);

            var wait = user.Action.UseSkill(_skill);

            yield return waitBeforeSelecting;
            user.Unselect(target);

            yield return wait;
        }
    }
}
