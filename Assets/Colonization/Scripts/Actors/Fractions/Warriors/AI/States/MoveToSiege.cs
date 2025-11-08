using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class WarriorAI
    {
        sealed private class MoveToSiege : AIState
        {
            private readonly bool _raider;

            private Hexagon _targetHexagon;
            private Key _targetColony;
            
            [Impl(256)] public MoveToSiege(WarriorAI parent) : base(parent)
            {
                _raider = s_settings.raiders[parent._actor.Id];
            }

            public override bool TryEnter() => _raider & Status.isMove && FindEnemiesColony();

            public override IEnumerator Execution_Cn(Out<bool> isContinue) => Move_Cn(isContinue, 0, _targetHexagon);

            public override void Dispose()
            {
                if (_targetHexagon != null)
                {
                    _targetHexagon = null;
                    Goals.Defensed.Remove(_targetColony);
                }
            }

            private bool FindEnemiesColony()
            {
                _targetHexagon = null;

                if (!Status.isSiege)
                {
                    int distance = s_settings.maxDistanceRaid;
                    var playerId = Actor.Owner;
                    for (int i = 0; i < PlayerId.HumansCount; i++)
                    {
                        if(GameContainer.Diplomacy.IsGreatEnemy(playerId, i) && TryGetEmptyColony(GetColonies(i), Goals.Raided, ref distance, out Crossroad colony, out Hexagon target))
                        {
                            _targetHexagon = target;
                            _targetColony = colony.Key;
                        }
                    }
                }
                return _targetHexagon != null && Goals.Raided.Add(_targetColony);
            }
        }
    }
}
