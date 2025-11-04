using System.Collections;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToColony : MoveTo
        {
            private Key _targetColony;

            public MoveToColony(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Action.CanUseMoveSkill() && FindEmptyColony() && Goals.Defensed.Add(_targetColony);

            public override IEnumerator Execution_Cn(Out<bool> isContinue) => Execution_Cn(isContinue, 0);

            public override void Dispose()
            {
                if(_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Defensed.Remove(_targetColony);
                }
            }

            private bool FindEmptyColony()
            {
                _targetHexagon = null;

                if (!Situation.isGuard || Situation.minColonyGuard > 1)
                {
                    Hexagon current, start = Actor.Hexagon; ; Crossroad colony;
                    ReadOnlyArray<Hexagon> hexagons;
                    int distance = s_settings.maxDistanceEmpty;
                    var colonies = Colonies;

                    for (int i = 0; i < colonies.Count; i++)
                    {
                        colony = colonies[i];
                        if (!Goals.Defensed.Contains(colony) && colony.IsEmptyNear(_playerId))
                        {
                            hexagons = colony.Hexagons;
                            foreach (int index in s_hexIndexes)
                            {
                                current = hexagons[index];
                                if (TryGetDistance(start, current, distance, out int newDistance))
                                {
                                    distance = newDistance;
                                    _targetHexagon = current;
                                    _targetColony = colony.Key;
                                }
                            }
                        }
                    }
                }
                return _targetHexagon != null;
            }
        }
    }
}
