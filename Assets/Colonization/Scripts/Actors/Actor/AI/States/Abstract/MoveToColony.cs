using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class MoveToColony : MoveTo
            {
                private readonly WeightsList<Target> _colonies = new();
                protected readonly List<int> _owners;
                protected Key _targetColony;
                
                [Impl(256)] protected MoveToColony(AI<TSettings, TActorId, TStateId> parent, int capacity) : base(parent) => _owners = new(capacity);

                protected bool TrySetColony(int maxDistance, int maxActors)
                {
                    _colonies.Clear();

                    int weightBase = (maxDistance + 1) * DISTANCE_RATE;
                    for (int i = _owners.Count - 1; i >= 0; --i)
                        SetEmptyColony(_owners[i], weightBase, maxDistance, maxActors);

                    while (_colonies.Count > 0)
                    {
                        var target = _colonies.Extract();
                        if (Goals.Colonies.Add(_targetColony = target.colony, Actor))
                        {
                            _targetHexagon = target.hexagon;
                            _colonies.Clear();
                        }
                    }

                    return _targetHexagon != null;
                }

                sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
                {
                    yield return Move_Cn(isContinue, 0, !_targetHexagon.IsEmpty, false, false);
                    if (!isContinue && IsEnemyComing)
                    {
                        isContinue.Set(true);
                        Exit();
                    }
                }

                sealed public override void Dispose()
                {
                    if (_targetHexagon != null)
                    {
                        _targetHexagon = null;
                        Goals.Colonies.Remove(_targetColony, Actor.Code);
                    }
                }

                private void SetEmptyColony(int playerId, int weightBase, int maxDistance, int maxActors)
                {
                    var colonies = GameContainer.Humans[playerId].Colonies;
                    ReadOnlyArray<Hexagon> hexagons;
                    Crossroad colony;

                    for (int i = 0; i < colonies.Count; ++i)
                    {
                        colony = colonies[i];
                        if (Goals.Colonies.CanAdd(colony, maxActors) && (colony.ApproximateDistance(Hexagon) <= (maxDistance + 1)) && colony.IsEmptyNear())
                        {
                            hexagons = colony.Hexagons;
                            foreach (int index in s_crossroadHex)
                            {
                                int distance = GetDistance(Actor, hexagons[index]);
                                if (distance > 0 && distance <= maxDistance)
                                    _colonies.Add(new(colony, hexagons[index]), weightBase - distance * DISTANCE_RATE);
                            }
                        }
                    }
                }

                // ********** Nested ***************
                private readonly struct Target
                {
                    public readonly Crossroad colony;
                    public readonly Hexagon hexagon;

                    public Target(Crossroad colony, Hexagon hexagon)
                    {
                        this.colony = colony;
                        this.hexagon = hexagon;
                    }
                }
            }
        }
    }
}
