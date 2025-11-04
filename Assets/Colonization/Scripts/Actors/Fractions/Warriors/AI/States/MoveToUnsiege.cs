using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToUnsiege : MoveTo
        {
            private ActorCode _targetEnemy;

            public MoveToUnsiege(WarriorAI parent) : base(parent) { }

            public override bool TryEnter() => Action.CanUseMoveSkill() && FindSiegedEnemy(Colonies) && Goals.Enemies.Add(_targetEnemy, new(Actor));

            public override IEnumerator Execution_Cn(Out<bool> isContinue) => Execution_Cn(isContinue, 1, !_targetHexagon.IsEnemy(_playerId));

            public override void Dispose()
            {
                if (_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Enemies.Remove(_targetEnemy, new(Actor.Code));
                }
            }

            private bool FindSiegedEnemy(ReadOnlyReactiveList<Crossroad> colonies)
            {
                _targetHexagon = null;
                Hexagon current, start = Actor.Hexagon; 
                ReadOnlyArray<Hexagon> hexagons;
                int distance = s_settings.maxDistanceUnsiege;

                for (int c = 0; c < colonies.Count; c++)
                {
                    hexagons = colonies[c].Hexagons;
                    for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                    {
                        current = hexagons[i];
                        if (current.IsEnemy(_playerId))
                        {
                            if (Goals.Enemies.CanAdd(current.Owner) && TryGetDistance(start, current, distance, out int newDistance))
                            {
                                distance = newDistance;
                                _targetHexagon = current;
                                _targetEnemy = current.Owner.Code;
                            }
                        }
                    }
                }
                return _targetHexagon != null;
            }
        }
    }
}
