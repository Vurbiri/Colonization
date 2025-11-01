using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToSieged : MoveTo
        {
            public MoveToSieged(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Action.CanUseMoveSkill() && FindSiegedEnemy(Actor, Colonies, out _target);

            public override IEnumerator Execution_Cn(Out<bool> isContinue) => Execution_Cn(isContinue, 1, !_target.IsEnemy(_playerId));

            private static bool FindSiegedEnemy(Actor actor, ReadOnlyReactiveList<Crossroad> colonies, out Hexagon target)
            {
                Hexagon current, start = actor.Hexagon; target = null;
                ReadOnlyArray<Hexagon> hexagons;
                int distance = s_settings.maxDistanceSiege, temp;

                for (int c = 0; c < colonies.Count; c++)
                {
                    hexagons = colonies[c].Hexagons;
                    for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                    {
                        current = hexagons[i];
                        if (current.IsEnemy(actor.Owner) && TryGetDistance(start, current, out temp) && temp < distance)
                        {
                            distance = temp;
                            target = current;
                        }
                    }
                }
                return target != null;
            }
        }
    }
}
