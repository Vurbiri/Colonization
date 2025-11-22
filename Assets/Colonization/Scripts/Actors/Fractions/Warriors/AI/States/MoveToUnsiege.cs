using System.Collections;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToUnsiege : State
        {
            private Hexagon _targetHexagon;
            private ActorCode _targetEnemy;

            [Impl(256)] public MoveToUnsiege(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Status.isMove && Status.percentHP > s_settings.minHPUnsiege)
                {
                    var player = GameContainer.Players.Humans[OwnerId];

                    int distance = CheckingColonies(player.Colonies, s_settings.maxDistanceUnsiege);
                    CheckingColonies(player.Ports, distance);
                }
                return _targetHexagon != null && Goals.Enemies.Add(_targetEnemy, new(Actor));

                #region Local: CheckingColonies(..), TargetSelection(..)
                // ===================================================
                int CheckingColonies(ReadOnlyReactiveList<Crossroad> colonies, int distance)
                {
                    for (int i = 0; i < colonies.Count; ++i)
                        distance = TargetSelection(colonies[i].Hexagons, distance);
                    return distance;
                }
                // ===================================================
                int TargetSelection(ReadOnlyArray<Hexagon> hexagons, int distance)
                {
                    Hexagon current;
                    for (int i = 0; i < Crossroad.HEX_COUNT; ++i)
                    {
                        current = hexagons[i];
                        if (current.IsEnemy(OwnerId))
                        {
                            if (Goals.Enemies.CanAdd(current.Owner) && TryGetDistance(Actor, current, distance, out int newDistance))
                            {
                                distance = newDistance;
                                _targetHexagon = current;
                                _targetEnemy = current.Owner.Code;
                            }
                        }
                    }
                    return distance;
                }
                // ===================================================
                #endregion
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 1, _targetHexagon, !_targetHexagon.IsEnemy(OwnerId));
                if (!isContinue && IsEnemyComing)
                    yield return Settings.defense.Use_Cn(Actor, true, true);
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
