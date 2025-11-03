using System.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class Defense : AIState
        {
            public Defense(WarriorAI parent) : base(parent)
            {
            }

            public override bool TryEnter() => Action.CanUseSpecSkill() && IsEnemyComing(_playerId, Actor.Hexagon.Key);

            public override void Dispose() { }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return GameContainer.CameraController.ToPositionControlled(Actor.Position);
                yield return Action.UseSpecSkill();
                isContinue.Set(false);
            }

            private static bool IsEnemyComing(Id<PlayerId> playerId, Key current)
            {
                bool result = false; Hexagon hex;

                for (int i = 0; !result & i < HEX.NEAR_TWO.Count; i++)
                    result = GameContainer.Hexagons.TryGet(current + HEX.NEAR_TWO[i], out hex) && hex.IsEnemy(playerId);

                return result;
            }
        }
    }
}
