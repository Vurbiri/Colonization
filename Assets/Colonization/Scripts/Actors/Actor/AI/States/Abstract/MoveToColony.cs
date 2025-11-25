using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class MoveToColony : MoveTo
            {
                protected const int MAX_COUNT = 3, COUNT_RATE = 20;

                private readonly WeightsList<Target> _colonies = new();
                private readonly int _weightBase;
                protected readonly List<int> _owners;
                protected Key _targetColony;

                [Impl(256)]
                protected MoveToColony(AI<TSettings, TActorId, TStateId> parent, int capacity, int maxDistance) : base(parent)
                {
                    _owners = new(capacity);
                    _weightBase = (maxDistance + 1) * (maxDistance) * DISTANCE_RATE + MAX_COUNT * MAX_COUNT * COUNT_RATE;
                }

                protected bool TrySetColony(int maxDistance)
                {
                    _colonies.Clear();

                    for (int i = _owners.Count - 1; i >= 0; --i)
                        SetEmptyColony(GameContainer.Humans[_owners[i]].Colonies, maxDistance);

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

                private void SetEmptyColony(ReadOnlyReactiveList<Crossroad> colonies, int maxDistance)
                {
                    ReadOnlyArray<Hexagon> hexagons;
                    Crossroad colony;

                    for (int c = 0, distance, index, count; c < colonies.Count; ++c)
                    {
                        colony = colonies[c];
                        count = Goals.Colonies.Count(colony);
                        if (count < MAX_COUNT && (colony.ApproximateDistance(Hexagon) <= (maxDistance + 1)) && colony.IsEmptyNear())
                        {
                            distance = maxDistance; index = -1;
                            hexagons = colony.Hexagons;
                            for (int h = 0, newDistance; h < Crossroad.HEX_COUNT; ++h)
                            {
                                if (TryGetDistance(Actor, hexagons[h], distance, out newDistance))
                                {
                                    distance = newDistance;
                                    index = h;
                                }
                            }
                            if(index >= 0)
                                _colonies.Add(new(colony, hexagons[index]), _weightBase - distance * distance * DISTANCE_RATE - count * count * COUNT_RATE);
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
