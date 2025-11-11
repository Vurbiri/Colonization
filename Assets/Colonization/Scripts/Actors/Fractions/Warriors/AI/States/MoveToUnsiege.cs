using System.Collections;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToUnsiege : AIState
        {
            private Hexagon _targetHexagon;
            private ActorCode _targetEnemy;

            [Impl(256)] public MoveToUnsiege(WarriorAI parent) : base(parent) {}

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Status.isMove && Status.percentHP > s_settings.minHPUnsiege)
                {

                    Hexagon current;
                    ReadOnlyArray<Hexagon> hexagons;
                    var colonies = Colonies;
                    int distance = s_settings.maxDistanceUnsiege;

                    for (int c = 0; c < colonies.Count; c++)
                    {
                        hexagons = colonies[c].Hexagons;
                        for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                        {
                            current = hexagons[i];
                            if (current.IsEnemy(Player))
                            {
                                if (Goals.Enemies.CanAdd(current.Owner) && TryGetDistance(Actor, current, distance, out int newDistance))
                                {
                                    distance = newDistance;
                                    _targetHexagon = current;
                                    _targetEnemy = current.Owner.Code;
                                }
                            }
                        }
                    }
                }
                return _targetHexagon != null && Goals.Enemies.Add(_targetEnemy, new(Actor));
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 1, _targetHexagon, !_targetHexagon.IsEnemy(Player));
                if (!isContinue && IsEnemyComing)
                    yield return Defense_Cn(true, true);
            }

            public override void Dispose()
            {
                if (_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Enemies.Remove(_targetEnemy, new(Actor.Code));
                }
            }

            
        }
    }
}
