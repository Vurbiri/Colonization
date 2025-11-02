using System.Collections;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToColony : MoveTo
        {
            public MoveToColony(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Action.CanUseMoveSkill() && FindEmptyColony();

            public override IEnumerator Execution_Cn(Out<bool> isContinue) => Execution_Cn(isContinue, 0);

            private bool FindEmptyColony()
            {
                _target = null;

                if (!Status.isGuard || Status.minGuard > 1)
                {
                    Hexagon current;
                    ReadOnlyArray<Hexagon> hexagons;
                    int distance = s_settings.maxDistanceEmpty;
                    var colonies = Colonies;

                    for (int i = 0; i < colonies.Count; i++)
                    {
                        if (colonies[i].IsEmptyNear(Actor.Owner))
                        {
                            hexagons = colonies[i].Hexagons;
                            foreach (int index in s_hexIndexes)
                            {
                                current = hexagons[index];
                                if (TryGetDistance(Actor.Hexagon, current, out int temp) && temp < distance)
                                {
                                    distance = temp;
                                    _target = current;
                                }
                            }
                        }
                    }
                }
                return _target != null;
            }
        }
    }
}
