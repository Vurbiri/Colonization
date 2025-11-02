using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        private abstract class MoveTo : AIState
        {
            protected Hexagon _target;

            protected MoveTo(WarriorAI parent) : base(parent)
            {
            }

            protected override void OnExit() => _target = null;

            protected IEnumerator Execution_Cn(Out<bool> isContinue, int distance, bool isExit = false)
            {
                isExit = isExit | Status.isInCombat;
                if (!isExit && Action.CanUseMoveSkill())
                {
                    isExit = !TryGetNextHexagon(Actor.Hexagon, _target, out Hexagon next);
                    if (!isExit)
                    {
                        yield return Move_Cn(next);
                        isExit = _target.Distance(next) == distance;
                    }
                }
                isContinue.Set(isExit);
                if (isExit)
                    Exit();
            }

            protected IEnumerator Move_Cn(Hexagon target)
            {
                yield return GameContainer.CameraController.ToPositionControlled(target.Position);

                var wait = Action.UseMoveSkill();

                yield return s_waitBeforeSelecting;
                Action.Unselect(target);

                yield return wait;
            }
        }
    }
}
