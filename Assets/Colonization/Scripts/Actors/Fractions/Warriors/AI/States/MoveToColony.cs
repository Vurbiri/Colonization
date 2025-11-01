using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToColony : MoveTo
        {
            public MoveToColony(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Action.CanUseMoveSkill() && FindEmptyColony(Actor, Colonies, out _target);

            public override IEnumerator Execution_Cn(Out<bool> isContinue) => Execution_Cn(isContinue, 0);

            private static bool FindEmptyColony(Actor actor, ReadOnlyReactiveList<Crossroad> colonies, out Hexagon target)
            {
                bool find = true; target = null;
                Hexagon start = actor.Hexagon;
                for (int i = 0; find & i < HEX.VERTICES; i++)
                    if (start.Crossroads[i].GetGuardCount(actor.Owner) == 1)
                        find = false;

                if (find)
                {
                    Hexagon current;
                    ReadOnlyArray<Hexagon> hexagons;
                    int distance = s_settings.maxDistanceEmpty, temp;

                    for (int i = 0; i < colonies.Count; i++)
                    {
                        if (colonies[i].IsEmptyNear(actor.Owner))
                        {
                            hexagons = colonies[i].Hexagons;
                            foreach (int index in s_haxIndexes)
                            {
                                current = hexagons[index];
                                if (TryGetDistance(start, current, out temp) && temp < distance)
                                {
                                    distance = temp;
                                    target = current;
                                }
                            }
                        }
                    }
                }
                return target != null;
            }
        }
    }
}
