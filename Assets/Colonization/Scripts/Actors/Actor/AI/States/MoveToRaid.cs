using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI<TSettings, TActorId, TStateId>
        {
            sealed private class MoveToRaid : State
            {
                private Hexagon _targetHexagon;
                private Key _targetColony;

                [Impl(256)] public MoveToRaid(AI<TSettings, TActorId, TStateId> parent) : base(parent) { }

                public override bool TryEnter()
                {
                    _targetHexagon = null;

                    if ((Settings.raider & Status.isMove & !Status.isSiege) && Status.percentHP > s_settings.minHPRaid)
                    {
                        int distance = s_settings.maxDistanceRaid;
                        var playerId = Actor.Owner;
                        for (int i = 0; i < PlayerId.HumansCount; ++i)
                        {
                            if (GameContainer.Diplomacy.IsGreatEnemy(playerId, i))
                            {
                                if (TryGetEmptyColony(i, ref distance, out Crossroad colony, out Hexagon target, Goals.Raid.CanAdd))
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
}
