using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToHelp : AIState
        {
            private Hexagon _targetHexagon;
            private ActorCode _targetEnemy;

            [Impl(256)] public MoveToHelp(WarriorAI parent) : base(parent) { }

            public override bool TryEnter()
            {
                _targetHexagon = null;

                if (Status.isMove && Status.percentHP > s_settings.minHPHelp)
                {
                    int distance = s_settings.maxDistanceHelp;
                    var playerId = Actor.Owner;

                    for (int i = 0; i < PlayerId.Count; i++)
                    {
                        if (GameContainer.Diplomacy.IsGreatFriend(playerId, i) && TryGetNearActorsInCombat(GameContainer.Actors[i], ref distance, out Actor enemy, out Actor friend))
                        {
                            _targetEnemy = enemy.Code;
                            _targetHexagon = (Support ? friend : enemy).Hexagon;
                        }
                    }
                }
                return _targetHexagon != null && Goals.Enemies.Add(_targetEnemy, new(Actor));
            }

            public override IEnumerator Execution_Cn(Out<bool> isContinue)
            {
                bool isExit = !(Support ? _targetHexagon.IsGreatFriend(Player) : _targetHexagon.IsEnemy(Player));
                yield return Move_Cn(isContinue, 1, _targetHexagon, isExit);
                if (!isContinue && IsEnemyComing)
                    yield return Defense_Cn(true, false);
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
