using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToRaid : AIState
        {
            private Hexagon _targetHexagon;
            private Key _targetColony;

            public override int Id => WarriorAIStateId.MoveToRaid;

            [Impl(256)] public MoveToRaid(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if ((Raider & Status.isMove & !Status.isSiege) && Status.percentHP > s_settings.minHPRaid)
                {
                    int distance = s_settings.maxDistanceRaid;
                    var playerId = Actor.Owner;
                    for (int i = 0; i < PlayerId.HumansCount; i++)
                    {
                        if (GameContainer.Diplomacy.IsGreatEnemy(playerId, i))
                        {
                            if (TryGetEmptyColony(GetColonies(i), ref distance, out Crossroad colony, out Hexagon target, Goals.Raid.CanAdd))
                            {
                                _targetHexagon = target;
                                _targetColony = colony.Key;
                            }
                        }
                    }
                }
                return _targetHexagon != null && Goals.Raid.Add(_targetColony, Actor.Code);
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                yield return Move_Cn(isContinue, 0, _targetHexagon, !_targetHexagon.CanWarriorEnter);
                if (!isContinue && IsEnemyComing)
                {
                    isContinue.Set(true);
                    Exit();
                }
            }

            public override void Dispose()
            {
                if (_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Raid.Remove(_targetColony, Actor.Code);
                }
            }
        }
    }
}
